namespace ProjectStorage.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string UploaderId { get; set; }

        public User Uploader { get; set; }

        [Required]
        public string Path { get; set; }

    }
}