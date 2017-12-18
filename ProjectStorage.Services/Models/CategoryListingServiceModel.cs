namespace ProjectStorage.Services.Models
{
    using Data.Models;
    using Infrastructure.Configuration;

    public class CategoryListingServiceModel : IMappableFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}