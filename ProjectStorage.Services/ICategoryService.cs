namespace ProjectStorage.Services
{
    using Models;
    using System.Collections.Generic;

    public interface ICategoryService
    {
        IEnumerable<CategoryListingServiceModel> GetAll();

        void Create(string name);

        void Edit(int id, string name);

        CategoryListingServiceModel GetById(int id);
        void Delete(int id);
    }
}