using ProjectStorage.Services.Models;

namespace ProjectStorage.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public interface IImageService
    {
        void Create (string getUserId, string title, IEnumerable<int> imageCategory, IFormFile imageImage);

        void Edit(string imageId, string title, IEnumerable<int> imageCategory);

        void Delete(string imageId);

        IEnumerable<ImageListingServiceModel> GetImagesByUser(string userId);

        ImagePageModel GetImagesDescendingByPage(int page);

        IEnumerable<ImageListingServiceModel> GetLikedImages(string userId);
        byte[] GetImage(string id);
        ImageListingServiceModel GetImageModel(string id);
    }
}