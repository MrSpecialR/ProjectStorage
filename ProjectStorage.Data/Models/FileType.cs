namespace ProjectStorage.Data.Models
{
    using System.Collections.Generic;

    public class FileType
    {
        public int Id { get; set; }

        public IList<File> Files { get; set; } = new List<File>();

        public string Extension { get; set; }
    }
}