namespace ProjectStorage.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Folder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public int? ParentId { get; set; }

        public Folder Parent { get; set; }

        public IList<Folder> Subfolders { get; set; } = new List<Folder>();

        public IList<File> Files { get; set; } = new List<File>();

        public bool IsInRootFolder { get; set; }
    }
}