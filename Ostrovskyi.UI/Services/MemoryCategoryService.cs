
using Cloth.Domain.Entities;
using Cloth.Domain.Models;


namespace Ostrovskyi.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
            new Category {Id=1, GroupName="Мужская",
            NormalizedName="Кожанная одежда для мужчин"},
            new Category {Id=2, GroupName="Женская",
            NormalizedName="Кожанная одежда для женщин"},
            new Category {Id=2, GroupName="Детская",
            NormalizedName="Кожанная одежда для детей"}

            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }

       
    }
}
