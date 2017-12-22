namespace ProjectStorage.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ImageService : IImageService
    {
        private readonly ProjectStorageDbContext db;

        private const string UserImagesPath = "~/../../Uploads/Images/Users/{0}";
        private const string ImagePath = "~/../../Uploads/Images/Users/{0}/{1}";

        public ImageService(ProjectStorageDbContext db)
        {
            this.db = db;
        }

        public void Create(string userId, string title, IEnumerable<int> imageCategory, IFormFile imageImage)
        {
            var imageId = Guid.NewGuid();
            var Image = new Image
            {
                Id = imageId,
                Title = title,
                Categories = imageCategory.Select(c => new CategoryImage
                {
                    ImageId = imageId,
                    CategoryId = c
                }).ToList(),
                UploaderId = userId,
                OriginalFileName = imageImage.FileName,
                UploadDate = DateTime.UtcNow,
                Path = string.Format(ImagePath, userId, imageId.ToString()) + '.' + imageImage.ContentType.Split('/')[1]
            };

            this.db.Images.Add(Image);
            this.db.SaveChanges();

            this.SaveFile(userId, imageId.ToString(), imageImage);
        }

        public void Edit(string imageId, string title, IEnumerable<int> imageCategory)
        {
            var Image = this.db.Images.FirstOrDefault(i => i.Id.ToString() == imageId);

            Image.Title = title;
            this.db.RemoveRange(this.db.CategoryImages.Where(ic => ic.ImageId == Image.Id).ToList());
            this.db.SaveChanges();

            this.db.AddRange(imageCategory.Select(c => new CategoryImage
            {
                CategoryId = c,
                ImageId = Image.Id
            }));
            this.db.SaveChanges();
        }

        public void Delete(string imageId)
        {
            var Image = this.db.Images.FirstOrDefault(i => i.Id.ToString() == imageId);
            if (Image == null)
            {
                return;
            }

            if (System.IO.File.Exists(Image.Path))
            {
                System.IO.File.Delete(Image.Path);
            }
            this.db.Images.Remove(Image);

            this.db.SaveChanges();
        }

        public IEnumerable<ImageListingServiceModel> GetImagesByUser(string userId)
        {
            return this.db.Images.Where(u => u.UploaderId == userId).ProjectTo<ImageListingServiceModel>().ToList();
        }

        public ImagePageModel GetImagesDescendingByPage(int page)
        {
            int pageCount = (int)Math.Ceiling(this.db.Images.Count() / 20.0);
            return new ImagePageModel
            {
                Images = this.db.Images.Skip((page - 1) * 20).Take(20).ProjectTo<ImageListingServiceModel>().ToList(),
                Pages = pageCount,
                CurrentPage = page
            };
        }

        public IEnumerable<ImageListingServiceModel> GetLikedImages(string userId)
        {
            return this.db.UserFavouriteImages.Where(u => u.UserId == userId).OrderByDescending(i => i.LikedDate).Select(ufi => ufi.Image).ProjectTo<ImageListingServiceModel>().ToList();
        }

        public byte[] GetImage(string id)
        {
            var image = this.db.Images.FirstOrDefault(i => i.Id.ToString() == id);
            return System.IO.File.ReadAllBytes(image.Path);
        }

        public ImageListingServiceModel GetImageModel(string id)
        {
            return this.db.Images.Where(f => f.Id.ToString() == id).ProjectTo<ImageListingServiceModel>()
                .FirstOrDefault();
        }

        public bool Exists(string id)
        {
            return this.db.Images.FirstOrDefault(i => i.Id.ToString() == id) != null;
        }

        public CategoryImageListingServiceModel GetImagesByCategory(int categoryId)
        {
            var category = this.db.Categories.Where(c => c.Id == categoryId).ProjectTo<CategoryImageListingServiceModel>().FirstOrDefault();
            if (category == null)
            {
                return null;
            }
            category.ImageList = this.db.Images.Where(i => i.Categories.Any(c => c.CategoryId == categoryId))
                .ProjectTo<ImageListingServiceModel>().ToList();
            return category;
        }

        public void LikeImage(string getUserId, string imageId)
        {
            if (this.db.UserFavouriteImages.Any(ufi => ufi.ImageId.ToString() == imageId && ufi.UserId == getUserId))
            {
                return;
            }

            this.db.UserFavouriteImages.Add(new UserFavouriteImages
            {
                UserId = getUserId,
                ImageId = Guid.Parse(imageId),
                LikedDate = DateTime.UtcNow
            });

            this.db.SaveChanges();
        }

        public void Dislike(string getUserId, string imageId)
        {
            if (this.db.UserFavouriteImages.Any(ufi => ufi.ImageId.ToString() == imageId && ufi.UserId == getUserId))
            {
                this.db.Remove(this.db.UserFavouriteImages.First(ufi =>
                    ufi.ImageId.ToString() == imageId && ufi.UserId == getUserId));
                this.db.SaveChanges();
            }
        }

        public IEnumerable<ImageListingServiceModel> GetAllImagesManage()
        {
            return this.db.Images.ProjectTo<ImageListingServiceModel>().ToList();
        }

        public ImageListingServiceModel GetImageById(string id)
        {
            return this.db.Images.Where(i => i.Id.ToString() == id).ProjectTo<ImageListingServiceModel>()
                .FirstOrDefault();
        }

        public bool LikesImage(string getUserId, string imageId)
        {
            return this.db.UserFavouriteImages.Any(ufi => ufi.UserId == getUserId && ufi.ImageId.ToString() == imageId);
        }

        private bool SaveFile(string userId, string imageId, IFormFile file)
        {
            string baseUsersDirectory = "~/../Uploads/Images/Users";
            if (!Directory.Exists(baseUsersDirectory))
            {
                Directory.CreateDirectory(baseUsersDirectory);
            }

            if (!Directory.Exists(string.Format(UserImagesPath, userId)))
            {
                Directory.CreateDirectory(string.Format(UserImagesPath, userId));
            }

            if (file.Length > 0)
            {
                using (var stream = new FileStream(string.Format(ImagePath, userId, imageId) + '.' + file.ContentType.Split('/')[1], FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return true;
            }
            return false;
        }
    }
}