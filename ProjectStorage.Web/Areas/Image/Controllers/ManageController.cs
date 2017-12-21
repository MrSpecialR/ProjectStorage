using ProjectStorage.Web.Constants;

namespace ProjectStorage.Web.Areas.Image.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProjectStorage.Data.Models;
    using ProjectStorage.Web.Areas.Image.Models.Image;
    using Services;
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

        // GET

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

            return View(image);
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

            if (this.userManager.GetUserId(this.User) != image.UploaderId && this.userManager.IsInRoleAsync(this.userManager.GetUserAsync(this.User).GetAwaiter().GetResult(), GlobalConstants.ImageModeratorRole).GetAwaiter().GetResult())
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

            if (this.userManager.GetUserId(this.User) != imageObj.UploaderId && this.userManager.IsInRoleAsync(this.userManager.GetUserAsync(this.User).GetAwaiter().GetResult(), GlobalConstants.ImageModeratorRole).GetAwaiter().GetResult())
            {
                return this.Redirect("/Account/Login");
            }

            this.imageService.Edit(id, image.Title, image.Category);

            return this.RedirectToAction("Index");
        }
    }
}