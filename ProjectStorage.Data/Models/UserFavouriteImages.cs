namespace ProjectStorage.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserFavouriteImages
    {
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public Guid ImageId { get; set; }

        public Image Image { get; set; }

        public DateTime LikedDate { get; set; }
    }
}