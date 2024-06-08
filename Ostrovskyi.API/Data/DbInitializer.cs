using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Cloth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ostrovskyi.API.Data
{

    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {

            // Uri проекта
            var uri = "https://localhost:7002/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //Выполнение миграций
            await context.Database.MigrateAsync();

            if (!context.Categories.Any() && !context.Kosuhi.Any())
            {
                var categories = new Category[]
            {
            new Category {GroupName="Мужская",
            NormalizedName="Кожанная одежда для мужчин"},
            new Category {GroupName="Женская",
            NormalizedName="Кожанная одежда для женщин"},
            new Category {GroupName="Детская",
            NormalizedName="Кожанная одежда для детей"}
            };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();


               var _kosuhi = new List<Kosuhi>
        {
            new Kosuhi {Name="Куртка 01",
            Description="Куртка ",
            Image= uri + "Images/09.png",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Кожанная одежда для детей"))},

            new Kosuhi {Name="Куртка 02",
            Description="Куртка ",
             Image= uri + "Images/02.png",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Кожанная одежда для женщин")) },

            new Kosuhi { Name="Куртка 03",
            Description="Куртка",
             Image= uri + "Images/03.png",
            Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("Кожанная одежда для женщин")) },

            new Kosuhi { Name="Куртка 04",
            Description="Куртка",
             Image= uri + "Images/04.png",
            Category =categories.FirstOrDefault(c=>c.NormalizedName.Equals("Кожанная одежда для мужчин")) },

            new Kosuhi { Name="Куртка 05",
            Description="Куртка",
             Image= uri + "Images/05.png",
            Category =categories.FirstOrDefault(c=>c.NormalizedName.Equals("Кожанная одежда для мужчин")) }
            };

                await context.Kosuhi.AddRangeAsync(_kosuhi);
                await context.SaveChangesAsync();

            }
        }
    }
}

