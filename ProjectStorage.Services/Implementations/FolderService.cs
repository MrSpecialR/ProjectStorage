namespace ProjectStorage.Services.Implementations
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
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


    }
}