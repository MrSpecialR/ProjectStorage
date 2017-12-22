namespace ProjectStorage.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Extensions;
    using Services;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.OkOrNotFound(this.categoryService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.Ok(this.categoryService.GetById(id));
        }
    }
}