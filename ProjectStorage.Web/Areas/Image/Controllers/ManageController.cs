namespace ProjectStorage.Web.Areas.Image.Controllers
{
    using Constants;
    using Data.Models;
    using Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.Image;
    using Services;
    using System;
    using System.Linq;

    public class ManageController : ImageBaseController
    {
        private readonly IImageService imageService;

        private readonly ICategoryService categoryService;

        private readonly UserManager<User> userManager;

        public ManageController(ICategoryService categoryService, IImageService imageService, UserManager<User> userManager)
        {
            this.categoryService = categoryService;
            this.imageService = imageService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Upload()
        {
            var image = new ImageCreateViewModel
            {
                Categories = this.categoryService.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            return this.View(image);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Upload(ImageCreateViewModel image)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Upload");
            }

            this.imageService.Create(this.userManager.GetUserId(this.User), image.Title, image.Category, image.Image);
            return this.RedirectToAction("Upload");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }
            var image = this.imageService.GetImageModel(id);

            if (image == null)
            {
                return this.NotFound();
            }

            if (this.userManager.GetUserId(this.User) != image.UploaderId && !this.userManager.IsInRoleAsync(this.userManager.GetUserAsync(this.User).GetAwaiter().GetResult(), GlobalConstants.ImageModeratorRole).GetAwaiter().GetResult())
            {
                return this.Redirect("/Account/Login");
            }

            return this.View(new ImageEditViewModel
            {
                Categories = this.categoryService.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Title = image.Title
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(string id, ImageEditViewModel image)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }
            var imageObj = this.imageService.GetImageModel(id);

            if (image == null)
            {
                return this.NotFound();
            }

            if (this.userManager.GetUserId(this.User) != imageObj.UploaderId && !this.userManager.IsInRoleAsync(this.userManager.GetUserAsync(this.User).GetAwaiter().GetResult(), GlobalConstants.ImageModeratorRole).GetAwaiter().GetResult())
            {
                return this.Redirect("/Account/Login");
            }

            this.imageService.Edit(id, image.Title, image.Category);

            return this.RedirectToAction("Index", "Home", new { area = "Image" });
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            if (!this.imageService.Exists(id))
            {
                return this.NotFound();
            }

            var image = this.imageService.GetImageModel(id);

            if (this.userManager.GetUserId(this.User) != image.UploaderId && !this.userManager.IsInRoleAsync(this.userManager.GetUserAsync(this.User).GetAwaiter().GetResult(), GlobalConstants.ImageModeratorRole).GetAwaiter().GetResult())
            {
                return this.Redirect("/Account/Login");
            }

            return this.View(new ImageEditViewModel
            {
                Title = image.Title,
                Categories = this.categoryService.GetCategoriesByImage(image.Id).Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            });
        }

        [ActionName("Delete")]
        [HttpPost]
        [Authorize]
        public IActionResult Delete_Post(string id)
        {
            if (!this.imageService.Exists(id))
            {
                return this.NotFound();
            }

            var image = this.imageService.GetImageModel(id);

            if (this.userManager.GetUserId(this.User) != image.UploaderId && !this.userManager.IsInRoleAsync(this.userManager.GetUserAsync(this.User).GetAwaiter().GetResult(), GlobalConstants.ImageModeratorRole).GetAwaiter().GetResult())
            {
                return this.Redirect("/Account/Login");
            }

            this.imageService.Delete(id);

            return this.RedirectToAction("Index", "Home", new { area = "Image" });
        }

        [Authorize]
        public IActionResult Like(string id)
        {
            this.imageService.LikeImage(this.userManager.GetUserId(this.User), id);
            try
            {
                var returnUrl = this.HttpContext.Request.Headers["Referer"].ToString();
                Uri url = new Uri(returnUrl);
                var localUrl = url.LocalPath;
                if (this.Url.IsLocalUrl(localUrl))
                {
                    return this.Redirect(returnUrl);
                }
            }
            catch
            {
                // ignored
            }
            return this.RedirectToAction("Index", "Home", new { area = "Image" });
        }

        [Authorize]
        public IActionResult Dislike(string id)
        {
            this.imageService.Dislike(this.userManager.GetUserId(this.User), id);
            try
            {
                var returnUrl = this.HttpContext.Request.Headers["Referer"].ToString();
                Uri url = new Uri(returnUrl);
                var localUrl = url.LocalPath;
                if (this.Url.IsLocalUrl(localUrl))
                {
                    return this.Redirect(returnUrl);
                }
            }
            catch
            {
                // ignored
            }
            return this.RedirectToAction("Index", "Home", new { area = "Image" });
        }

        [Authorize(Roles = GlobalConstants.ImageModeratorRole)]
        public IActionResult Moderate()
        {
            return this.View(this.imageService.GetAllImagesManage());
        }

        public IActionResult Details(string id)
        {
            return this.ViewOrNotFound(this.imageService.GetImageById(id));
        }

        [Authorize]
        public IActionResult MyLikes()
        {
            return this.ViewOrNotFound(this.imageService.GetLikedImages(this.userManager.GetUserId(this.User)));
        }
    }
}