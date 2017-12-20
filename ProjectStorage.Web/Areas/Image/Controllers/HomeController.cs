namespace ProjectStorage.Web.Areas.Image.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class HomeController : ImageBaseController
    {

        private readonly IImageService imageService;

        private readonly ICategoryService categoryService;

        private readonly UserManager<User> userManager;

        public HomeController(UserManager<User> userManager, ICategoryService categoryService, IImageService imageService)
        {
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.imageService = imageService;
        }

        public IActionResult Index(int page = 1)
        {
            if (page < 0)
            {
                page = 1;
            }
            return View(this.imageService.GetImagesDescendingByPage(page));
        }

        public IActionResult Image(string id)
        {
            return this.File(this.imageService.GetImage(id), "image/png");
        }
    }
}