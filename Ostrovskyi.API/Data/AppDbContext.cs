using Microsoft.EntityFrameworkCore;
using Cloth.Domain.Entities;


namespace Ostrovskyi.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Kosuhi> Kosuhi { get; set; }
        public DbSet<Category> Categories { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



    }
}
