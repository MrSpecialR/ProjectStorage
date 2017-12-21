namespace ProjectStorage.Web.Areas.Image.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class CategoryController : ImageBaseController
    {
        private readonly IImageService imageService;
        private readonly ICategoryService categoryService;


        public CategoryController(IImageService imageService, ICategoryService categoryService)
        {
            this.imageService = imageService;
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return this.View(this.categoryService.GetAll());
        }

        public IActionResult Browse(int id)
        {
            return this.ViewOrNotFound(this.imageService.GetImagesByCategory(id));
        }
    }
}