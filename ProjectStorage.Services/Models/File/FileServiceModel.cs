namespace ProjectStorage.Services.Models.File
{
    using Infrastructure.Configuration;
    using System;

    public class FileServiceModel : IMappableFrom<Data.Models.File>
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