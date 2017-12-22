namespace ProjectStorage.Services.Models
{
    using Data.Models;
    using Infrastructure.Configuration;
    using System.ComponentModel.DataAnnotations;

    public class CategoryListingServiceModel : IMappableFrom<Category>
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Required]
        public string Name { get; set; }
    }
}