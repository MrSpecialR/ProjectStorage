namespace ProjectStorage.Services
{
    using Microsoft.AspNetCore.Http;
    using Models;
    using System.Collections.Generic;

    public interface IImageService
    {
        void Create(string getUserId, string title, IEnumerable<int> imageCategory, IFormFile imageImage);

        void Edit(string imageId, string title, IEnumerable<int> imageCategory);

        void Delete(string imageId);

        IEnumerable<ImageListingServiceModel> GetImagesByUser(string userId);

        ImagePageModel GetImagesDescendingByPage(int page);

        IEnumerable<ImageListingServiceModel> GetLikedImages(string userId);

        byte[] GetImage(string id);

        ImageListingServiceModel GetImageModel(string id);

        bool Exists(string id);

        CategoryImageListingServiceModel GetImagesByCategory(int categoryId);

        void LikeImage(string getUserId, string imageId);

        void Dislike(string getUserId, string imageId);

        IEnumerable<ImageListingServiceModel> GetAllImagesManage();

        ImageListingServiceModel GetImageById(string id);

        bool LikesImage(string getUserId, string imageId);
    }
}