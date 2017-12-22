namespace ProjectStorage.Services.Models.Image
{
    using System.Collections.Generic;

    public class ImagePageModel
    {
        public IEnumerable<ImageListingServiceModel> Images { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; }
    }
}