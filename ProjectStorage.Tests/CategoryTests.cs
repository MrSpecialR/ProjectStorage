namespace ProjectStorage.Tests
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using Services.Implementations;
    using Services.Models.Category;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class CategoryTests : BaseTest
    {
        private ProjectStorageDbContext GetDatabase()
        {
            DbContextOptions<ProjectStorageDbContext> dbOptions = new DbContextOptionsBuilder<ProjectStorageDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ProjectStorageDbContext(dbOptions);
        }

        private ICategoryService GetCategoryService()
        {
            return new CategoryService(this.GetDatabase());
        }

        private ICategoryService GetCategoryService(ProjectStorageDbContext context)
        {
            return new CategoryService(context);
        }

        private IList<Category> GetTestData()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Category 1"
                },
                new Category
                {
                    Id = 2,
                    Name = "Category 2"
                },
                new Category
                {
                    Id = 3,
                    Name = "Category 3"
                },
                new Category
                {
                    Id = 4,
                    Name = "Category 4"
                },
            };
        }

        [Fact]
        public void APIControllerGetShouldReturnAllData()
        {
            //Arrange
            var db = this.GetDatabase();
            var content = this.GetTestData();
            var service = this.GetCategoryService(db);
            db.Categories.AddRange(content);
            var contentAsServicemodel = content.AsQueryable().ProjectTo<CategoryListingServiceModel>().ToList();

            //Act
            var returnedVal = service.GetAll();

            //Assert
            returnedVal.Should().OnlyContain(c => contentAsServicemodel.Any(csm => csm.Id == c.Id) && contentAsServicemodel.Any(csm => csm.Name == c.Name));
        }
    }
}
