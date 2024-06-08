using Cloth.Domain.Entities;
using Cloth.Domain.Models;
using Ostrovskyi.UI.Controllers;
using Ostrovskyi.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.TESTS
{
    public class ProductControllerTests
    {
        IProductService _productService;
        ICategoryService _categoryService;
        public ProductControllerTests()
        {

        }
        // Список категорий сохраняется во ViewData
        [Fact]
        public async void IndexPutsCategoriesToViewData()
        {
            //arrange
            var controller = new ProductController((IProductService)_categoryService, (ICategoryService)_productService);
            //act
            var response = await controller.Index(null);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            var categories = Assert.IsType<List<Category>>(view.ViewData["categories"]);
            Assert.Equal(6, categories.Count);
            Assert.Equal("Все", view.ViewData["currentCategory"]);
        }
        // Имя текущей категории сохраняется во ViewData
        [Fact]
        public async void IndexSetsCorrectCurrentCategory()
        {
            //arrange
            var categories = await _categoryService.GetCategoryListAsync();
            var currentCategory = categories.Data[0];
            var controller = new ProductController((IProductService)_categoryService, (ICategoryService)_productService);
            //act
            var response = await controller.Index(currentCategory.NormalizedName);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            Assert.Equal(currentCategory.GroupName, view.ViewData["currentCategory"]);
        }
        // В случае ошибки возвращается NotFoundObjectResult
        [Fact]
        public async void IndexReturnsNotFound()
        {
            //arrange
            string errorMessage = "Test error";
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Success = false;
            categoriesResponse.ErrorMessage = errorMessage;
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
            ;
            var controller = new ProductController((IProductService)_categoryService, (ICategoryService)_productService);
            //act
            var response = await controller.Index(null);
            //assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(errorMessage, result.Value.ToString());

            // Настройка имитации ICategoryService и IProductService
            void SetupData()
            {
                _categoryService = Substitute.For<ICategoryService>();
                var categoriesResponse = new ResponseData<List<Category>>();
                categoriesResponse.Data = new List<Category>
           {
            new Category {Id=1, GroupName="Мужская",
            NormalizedName="Кожанная одежда для мужчин"},
            new Category {Id=2, GroupName="Женская",
            NormalizedName="Кожанная одежда для женщин"},
            new Category {Id=2, GroupName="Детская",
            NormalizedName="Кожанная одежда для детей"}

            };
                _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse))
                ;
                _productService = Substitute.For<IProductService>();
                var amges = new List<Kosuhi>
            {
            new Kosuhi {Id = 1 },
            new Kosuhi {Id = 2 },
            new Kosuhi { Id = 3 },
            new Kosuhi { Id = 4 },
            new Kosuhi { Id = 5 }
            };
                var productResponse = new ResponseData<ListModel<Kosuhi>>();
                productResponse.Data = new ListModel<Kosuhi> { Items = amges };
                _productService.GetProductListAsync(Arg.Any<string?>(), Arg.Any<int>())
                .Returns(productResponse);
            }
        }
    }
}


