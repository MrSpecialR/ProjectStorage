using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ProjectStorage.Data.Models;
using File = ProjectStorage.Data.Models.File;

namespace ProjectStorage.Services.Implementations
{
    using Data;
    using Microsoft.AspNetCore.Http;

    public class ProjectService : IProjectService
    {

        private IDictionary<string, File> files;
        private IDictionary<string, Folder> folders;



        private readonly ProjectStorageDbContext db;

        private const string ProjectsFolder = "~/../../Uploads/Projects/";

        public ProjectService(ProjectStorageDbContext db)
        {
            this.db = db;
            this.files = new Dictionary<string, File>();
            this.folders = new Dictionary<string, Folder>();
        }

        public void Add(string userId, IFormFile file)
        {
            Guid id = Guid.NewGuid();
            string projectFolder = ProjectsFolder + "/" + id;
            Directory.CreateDirectory(projectFolder);
            List<FileType> filetypes = this.db.FileTypes.ToList();

            var project = new Project
            {
                Name = "Default Name for now",
                IsPublic = false,
                UploaderId = userId,
                UploadDate = DateTime.UtcNow
            };

            this.db.Projects.Add(project);

            this.db.SaveChanges();

            this.SaveProject(file, projectFolder);

            this.folders.Add(projectFolder, new Folder
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                IsInRootFolder = false
            });
            this.ProcessDirectory(projectFolder, project.Id, filetypes);

            this.db.AddRange(this.folders.Values);
            this.db.AddRange(this.files.Values);

            this.db.SaveChanges();
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

        public void ProcessDirectory(string targetDirectory, int projectId, List<FileType> filetypes)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                this.ProcessFile(fileName, projectId, targetDirectory, filetypes);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                this.folders.Add(subdirectory, new Folder
                {
                    Id = Guid.NewGuid(),
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
                FolderId = this.folders[targetDirectory].ParentId == null && !this.folders[targetDirectory].IsInRootFolder ? (Guid?) null : this.folders[targetDirectory].Id,
                Path = path,
                Name = path.Split('/').Last(),
                ProjectId = projectId,
                FileTypeId = filetypes.FirstOrDefault(ft => ft.Extension==extension).Id
            });
        }
    }
}