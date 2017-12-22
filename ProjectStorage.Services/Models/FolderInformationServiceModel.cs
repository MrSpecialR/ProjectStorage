namespace ProjectStorage.Services.Models
{
    using Data.Models;
    using Infrastructure.Configuration;
    using System;
    using System.Collections.Generic;

    public class FolderInformationServiceModel : IMappableFrom<Folder>
    {
        public Guid Id { get; set; }

        public int ProjectId { get; set; }

        public string FolderName { get; set; }

        public Guid? ParentId { get; set; }

        public IEnumerable<SubfolderServiceModel> SubfolderIdentifiers { get; set; }

        public IEnumerable<FileServiceModel> FilesInFolder { get; set; }

        public bool IsInRootFolder { get; set; }
    }
}