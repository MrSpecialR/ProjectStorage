using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectStorage.Data.Models;
using ProjectStorage.Services.Models;
using ProjectStorage.Web.Areas.Image.Models.Image;

namespace ProjectStorage.Web.Areas.Image.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class ImageController : ImageBaseController
    {
        private readonly IImageService imageService;

        private readonly ICategoryService categoryService;

        private readonly UserManager<User> userManager;

        public ImageController(ICategoryService categoryService, IImageService imageService, UserManager<User> userManager)
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
    }
}