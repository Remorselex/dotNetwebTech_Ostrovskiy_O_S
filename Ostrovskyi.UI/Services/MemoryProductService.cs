using Cloth.Domain.Entities;
using Cloth.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace Ostrovskyi.UI.Services
{
    
        public class MemoryProductService: IProductService
        {
            List<Kosuhi> _kosuhi;
            List<Category> _categories;
            IConfiguration _config;



            public MemoryProductService(ICategoryService categoryService, [FromServices] IConfiguration config)
            {
                _config = config;
                _categories = categoryService.GetCategoryListAsync()
                    .Result
                    .Data;

                SetupData();
            }



        /// <summary>
        /// Инициализация списков
        /// </summary>
        public void SetupData()
        {

            _kosuhi = new List<Kosuhi>
        {
            new Kosuhi {Id = 1, Name="Куртка 01",
            Description="Куртка ",
            Image="Images/01.png",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Кожанная одежда для женщин")).Id},

            new Kosuhi { Id = 2, Name="Куртка 02",
            Description="Куртка ",
             Image="Images/02.png",
            CategoryId=_categories.Find(c=>c.NormalizedName.Equals("Кожанная одежда для женщин")).Id },

            new Kosuhi { Id = 2, Name="Куртка 03",
            Description="Куртка",
             Image="Images/03.png",
            CategoryId=_categories.Find(c=>c.NormalizedName.Equals("Кожанная одежда для женщин")).Id },

            new Kosuhi { Id = 2, Name="Куртка 04",
            Description="Куртка",
             Image="Images/04.png",
            CategoryId=_categories.Find(c=>c.NormalizedName.Equals("Кожанная одежда для мужчин")).Id },

            new Kosuhi { Id = 2, Name="Куртка 05",
            Description="Куртка",
             Image="Images/05.png",
            CategoryId=_categories.Find(c=>c.NormalizedName.Equals("Кожанная одежда для мужчин")).Id }
            };

        }
        Task<ResponseData<ListModel<Kosuhi>>> IProductService.GetProductListAsync(string? categoryNormalizedName, int pageNo=1)
        {


                // Создать объект результата
                var result = new ResponseData<ListModel<Kosuhi>>();

                // Id категории для фильрации
                int? categoryId = null;

                // если требуется фильтрация, то найти Id категории
                // с заданным categoryNormalizedName
                if (categoryNormalizedName != null)
                    categoryId = _categories
                    .Find(c =>
                    c.NormalizedName.Equals(categoryNormalizedName))
                    ?.Id;

                // Выбрать объекты, отфильтрованные по Id категории,
                // если этот Id имеется
                var data = _kosuhi
                .Where(d => categoryNormalizedName == null || d.CategoryId.Equals(categoryNormalizedName))?
                .ToList();

                // получить размер страницы из конфигурации
                int pageSize = _config.GetSection("ItemsPerPage").Get<int>();


                // получить общее количество страниц
                int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

                // получить данные страницы
                var listData = new ListModel<Kosuhi>()
                {
                    Items = data.Skip((pageNo - 1) *
                pageSize).Take(pageSize).ToList(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages
                };

                // поместить ранные в объект результата
                result.Data = listData;



                // Если список пустой
                if (data.Count == 0)
                {
                    result.Success = false;
                    result.ErrorMessage = "Нет объектов в выбраннной категории";
                }
                // Вернуть результат
                return Task.FromResult(result);

            }



        public Task<ResponseData<Kosuhi>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Kosuhi product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Kosuhi>> CreateProductAsync(Kosuhi product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }


    }
    }
