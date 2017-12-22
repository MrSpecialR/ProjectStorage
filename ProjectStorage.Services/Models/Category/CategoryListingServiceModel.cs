namespace ProjectStorage.Services.Models.Category
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Configuration;

    public class CategoryListingServiceModel : IMappableFrom<Data.Models.Category>
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Required]
        public string Name { get; set; }
    }
}