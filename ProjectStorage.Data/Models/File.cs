namespace ProjectStorage.Data.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int FileTypeId { get; set; }
        public FileType FileType { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public int? FolderId { get; set; }

        public Folder Folder { get; set; }

        public bool IsInRootFolder { get; set; }
    }
}