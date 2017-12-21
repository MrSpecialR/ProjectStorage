namespace ProjectStorage.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        public int FileTypeId { get; set; }
        public FileType FileType { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid? FolderId { get; set; }

        public Folder Folder { get; set; }

        public bool IsInRootFolder { get; set; }
    }
}