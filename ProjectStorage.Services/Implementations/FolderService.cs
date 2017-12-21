using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ProjectStorage.Services.Implementations
{
    using System.Linq;
    using AutoMapper;
    using Data;
    using Models;

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
            returnedObject.SubfolderIdentifiers = this.db.Folders.Where(f => f.ParentId == folderModel.Id).ProjectTo<SubfolderServiceModel>().ToList();

            returnedObject.FilesInFolder = folderModel.Files.Select(f => this.mapper.Map<FileServiceModel>(f)).ToList();

            return returnedObject;
        }
    }
}