namespace ProjectStorage.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public string UploaderId { get; set; }

        public User Uploader { get; set; }

        public IList<Folder> Folders { get; set; } = new List<Folder>();

        public IList<File> Files { get; set; } = new List<File>();

        public DateTime UploadDate { get; set; }

        public bool IsPublic { get; set; }
    }
}