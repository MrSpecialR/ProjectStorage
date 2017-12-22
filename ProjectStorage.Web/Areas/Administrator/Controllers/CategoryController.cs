using ProjectStorage.Services.Models.Category;

namespace ProjectStorage.Web.Areas.Administrator.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Models.Category;
    using Services;
    using Services.Models;

    public class CategoryController : AdministratorBaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult All()
        {
            return View(this.categoryService.GetAll());
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel category)
        {
            if (!ModelState.IsValid)
            {
                return this.View(category);
            }

            this.categoryService.Create(category.Name);

            return this.RedirectToAction("All");
        }

        public IActionResult Edit(int id)
        {
            return this.ViewOrNotFound(this.categoryService.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(int id, CategoryViewModel category)
        {
            if (!ModelState.IsValid)
            {
                return this.View(new CategoryListingServiceModel
                {
                    Id = id,
                    Name = category.Name
                });
            }

            this.categoryService.Edit(id, category.Name);

            return this.RedirectToAction("All");
        }

        public IActionResult Delete(int id)
        {
            return this.ViewOrNotFound(this.categoryService.GetById(id));
        }

        [ActionName("Delete")]
        [HttpPost]
        public IActionResult Delete_Post(int id)
        {
            this.categoryService.Delete(id);

            return this.RedirectToAction("All");
        }
    }
}