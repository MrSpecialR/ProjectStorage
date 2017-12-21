namespace ProjectStorage.Services.Models
{
    using Data.Models;
    using Infrastructure.Configuration;
    using System;

    public class FileServiceModel : IMappableFrom<File>
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Path { get; set; }

        public int FileTypeId { get; set; }

        public int ProjectId { get; set; }
        
        public Guid? FolderId { get; set; }

        public bool IsInRootFolder { get; set; }

        public byte[] Content { get; set; }
    }
}