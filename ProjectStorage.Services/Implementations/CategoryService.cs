namespace ProjectStorage.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models.Category;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoryService : ICategoryService
    {
        private readonly ProjectStorageDbContext db;

        public CategoryService(ProjectStorageDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CategoryListingServiceModel> GetAll()
        {
            return this.db.Categories.ProjectTo<CategoryListingServiceModel>().ToList();
        }

        public void Create(string name)
        {
            this.db.Categories.Add(new Category
            {
                Name = name
            });
            this.db.SaveChanges();
        }

        public void Edit(int id, string name)
        {
            var category = this.db.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return;
            }

            category.Name = name;

            this.db.SaveChanges();
        }

        public CategoryListingServiceModel GetById(int id)
        {
            return this.db.Categories.Where(c => c.Id == id).ProjectTo<CategoryListingServiceModel>().FirstOrDefault();
        }

        public void Delete(int id)
        {
            var category = this.db.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return;
            }

            this.db.Remove(category);

            this.db.SaveChanges();
        }

        public IEnumerable<CategoryListingServiceModel> GetCategoriesByImage(string imageId)
        {
            return this.db.Categories.Where(c => c.Images.Any(i => i.ImageId.ToString() == imageId)).ProjectTo<CategoryListingServiceModel>().ToList();
        }
    }
}