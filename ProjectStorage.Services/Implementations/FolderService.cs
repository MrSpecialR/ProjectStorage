using System.Collections.Generic;
using ProjectStorage.Services.Models.File;
using ProjectStorage.Services.Models.Folder;

namespace ProjectStorage.Services.Implementations
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    public class FolderService : IFolderService
    {
        private readonly ProjectStorageDbContext db;
        private readonly IMapper mapper;

        public FolderService(ProjectStorageDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public IEnumerable<FolderListingServiceModel> GetFolders()
        {
            return this.db.Folders.ProjectTo<FolderListingServiceModel>().ToList();
        }

        public FolderInformationServiceModel GetFolder(string id)
        {
            var folderModel = this.db.Folders.Include("Files").FirstOrDefault(f => f.Id.ToString() == id);
            if (folderModel == null)
            {
                return null;
            }
            var returnedObject = this.mapper.Map<FolderInformationServiceModel>(folderModel);
            returnedObject.SubfolderIdentifiers = this.db.Folders.Where(f => f.ParentId == folderModel.Id)
                .ProjectTo<SubfolderServiceModel>().ToList();

            returnedObject.FilesInFolder = folderModel.Files.Select(f => this.mapper.Map<FileServiceModel>(f)).ToList();

            return returnedObject;
        }

        public byte[] ZipFolder(string id)
        {
            var directory = this.db.Folders.FirstOrDefault(f => f.Id.ToString() == id);

            if (directory == null)
            {
                return null;
            }
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    Zipper.ProcessDirectory(directory.Path, archive);
                }
                return memoryStream.ToArray();
            }
        }

        public void Delete(string folderId)
        {
            var folder = this.db.Folders.Include("Parent").FirstOrDefault(f => f.Id.ToString() == folderId);
            if (folder == null)
            {
                return;
            }

            var filesInFolder = this.db.Files.Where(f => f.FolderId.HasValue && f.FolderId.Value.ToString() == folderId).ToList();
            this.db.RemoveRange(filesInFolder);
            this.db.SaveChanges();

            folder.Parent.Subfolders.Remove(folder);
            folder.Parent = null;

            this.db.SaveChanges();

            this.db.Remove(folder);
            this.db.SaveChanges();
        }

        public bool IsOwner(string userId, string folderId)
        {
            var uploader = this.db.Folders.Where(f => f.Id.ToString() == folderId).Select(f => f.Project.UploaderId).FirstOrDefault();
            return uploader == userId;
        }
    }
}