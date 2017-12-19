namespace ProjectStorage.Data.Models
{
    using System;
    using System.Collections.Generic;
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
        [StringLength(100, MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string OriginalFileName { get; set; }

        public IList<UserFavouriteImages> UsersLiked { get; set; } = new List<UserFavouriteImages>();

        public IList<CategoryImage> Categories { get; set; } = new List<CategoryImage>();

        public DateTime UploadDate { get; set; }

        [Required]
        public string Path { get; set; }
    }
}