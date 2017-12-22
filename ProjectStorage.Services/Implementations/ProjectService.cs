namespace ProjectStorage.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using File = Data.Models.File;

    public class ProjectService : IProjectService
    {
        private readonly IDictionary<string, File> files;
        private readonly IDictionary<string, Folder> folders;

        private readonly ProjectStorageDbContext db;

        private const string ProjectsFolder = "~/../../Uploads/Projects/";

        public ProjectService(ProjectStorageDbContext db)
        {
            this.db = db;
            this.files = new Dictionary<string, File>();
            this.folders = new Dictionary<string, Folder>();
        }

        public FolderInformationServiceModel GetProject(int id)
        {
            var project = this.db.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return null;
            }

            var folder = new FolderInformationServiceModel
            {
                FilesInFolder = this.db.Files.Where(f => f.ProjectId == id && f.IsInRootFolder).ProjectTo<FileServiceModel>().ToList(),
                FolderName = project.Name,
                SubfolderIdentifiers = this.db.Folders.Where(f => f.ProjectId == id && f.IsInRootFolder).ProjectTo<SubfolderServiceModel>().ToList()
            };

            return folder;
        }

        public int Add(string userId, IFormFile file, string fileTitle)
        {
            Guid id = Guid.NewGuid();
            string projectFolder = ProjectsFolder + "/" + id;
            Directory.CreateDirectory(projectFolder);
            List<FileType> filetypes = this.db.FileTypes.ToList();

            var root = Guid.NewGuid();

            var project = new Project
            {
                Name = fileTitle,
                IsPublic = true,
                UploaderId = userId,
                RootFolderName = id.ToString(),
                UploadDate = DateTime.UtcNow
            };

            this.db.Projects.Add(project);

            this.db.SaveChanges();

            this.SaveProject(file, projectFolder);

            this.folders.Add(projectFolder, new Folder
            {
                Id = root,
                FolderName = Path.GetFileName(projectFolder),
                Path = projectFolder,
                ProjectId = project.Id,
                IsInRootFolder = false
            });
            this.ProcessDirectory(projectFolder, project.Id, filetypes);

            this.folders.Values.Where(f => f.ParentId == root).ToList().ForEach(f =>
            {
                f.ParentId = null;
                f.IsInRootFolder = true;
            });

            this.folders.Remove(projectFolder);

            this.db.AddRange(this.folders.Values);
            this.db.AddRange(this.files.Values);

            this.db.SaveChanges();

            return project.Id;
        }

        public void Delete(int projectId)
        {
            //   var folder = this.db.Projects.FirstOrDefault(p => p.Id == projectId).RootFolderName;
            var folder = this.GetFolderNameByProjectId(projectId);
            this.db.RemoveRange(this.db.Files.Where(f => f.IsInRootFolder && f.ProjectId == projectId).ToList());
            this.db.RemoveRange(this.db.Folders.Where(f => f.ProjectId == projectId).ToList());
            this.db.SaveChanges();
            this.db.Projects.Remove(this.db.Projects.FirstOrDefault(p => p.Id == projectId));
            this.db.SaveChanges();
            var folderToDelete = ProjectsFolder + folder;
            Directory.Delete(folderToDelete, true);
        }

        public IEnumerable<ProjectListingModel> GetAllProjects()
        {
            return this.db.Projects.ProjectTo<ProjectListingModel>().ToList();
        }

        private string GetFolderNameByProjectId(int id)
        {
            var project = this.db.Projects.Include("Files").FirstOrDefault(p => p.Id == id);
            var firstFile = project.Files.FirstOrDefault();
            var path = firstFile.Path;
            var parts = path.Split(new[] { ProjectsFolder }, StringSplitOptions.RemoveEmptyEntries).First().Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            return parts;
        }

        private void SaveProject(IFormFile formFile, string projectFolder)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                using (ZipArchive zip = new ZipArchive(memoryStream))
                {
                    foreach (var zipFile in zip.Entries)
                    {
                        List<string> zipDirectories = zipFile.FullName.Split('/').ToList();
                        int countOfNestedDirectories = zipDirectories.Count - 1;
                        for (int i = 0; i < countOfNestedDirectories; i++)
                        {
                            Directory.CreateDirectory(projectFolder + "/" + string.Join("/", zipDirectories.Take(i + 1)));
                        }

                        if (zipFile.FullName.EndsWith("/"))
                        {
                            continue;
                        }

                        using (StreamWriter writer =
                            new StreamWriter(projectFolder + "/" + zipFile.FullName))
                        {
                            using (StreamReader sr = new StreamReader(zipFile.Open()))
                            {
                                writer.Write(sr.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }

        public byte[] ZipProject(int id)
        {
            var directory = "~/../../Uploads/Projects//" + this.db.Projects.FirstOrDefault(p => p.Id == id).RootFolderName;

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    Zipper.ProcessDirectory(directory, archive);
                }
                return memoryStream.ToArray();
            }
        }

        public bool UserIsOwner(int id, string userID)
        {
            return this.db.Projects.Any(p => p.UploaderId == userID && p.Id == id);
        }

        private void ProcessDirectory(string targetDirectory, int projectId, List<FileType> filetypes)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                this.ProcessFile(fileName, projectId, targetDirectory, filetypes);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                var folderName = Path.GetFileName(subdirectory);
                this.folders.Add(subdirectory, new Folder
                {
                    Id = Guid.NewGuid(),
                    FolderName = folderName,
                    Path = this.folders[targetDirectory].Path + "/" + folderName,
                    ParentId = this.folders[targetDirectory].Id,
                    IsInRootFolder = this.folders[targetDirectory].ParentId == null && !this.folders[targetDirectory].IsInRootFolder,
                    ProjectId = projectId
                });
                this.ProcessDirectory(subdirectory, projectId, filetypes);
            }
        }

        private void ProcessFile(string path, int projectId, string targetDirectory, List<FileType> filetypes)
        {
            string extension = Path.GetExtension(path);
            if (filetypes.All(ft => ft.Extension != extension))
            {
                var newExtensionType = new FileType
                {
                    Extension = extension
                };
                this.db.FileTypes.Add(newExtensionType);
                filetypes.Add(newExtensionType);
                this.db.SaveChanges();
            }

            this.files.Add(path, new File
            {
                Id = Guid.NewGuid(),
                FolderId = this.folders[targetDirectory].ParentId == null && !this.folders[targetDirectory].IsInRootFolder ? (Guid?)null : this.folders[targetDirectory].Id,
                IsInRootFolder = this.folders[targetDirectory].ParentId == null && !this.folders[targetDirectory].IsInRootFolder,
                Path = path,
                Name = path.Split('\\').Last(),
                ProjectId = projectId,
                FileTypeId = filetypes.FirstOrDefault(ft => ft.Extension == extension).Id
            });
        }
    }
}