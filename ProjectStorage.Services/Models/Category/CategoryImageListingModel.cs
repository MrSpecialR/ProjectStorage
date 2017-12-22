using ProjectStorage.Services.Models.Image;

namespace ProjectStorage.Services.Models.Category
{
    using Data.Models;
    using Infrastructure.Configuration;
    using System.Collections.Generic;

    public class CategoryImageListingServiceModel : IMappableFrom<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ImageListingServiceModel> ImageList { get; set; }
    }
}