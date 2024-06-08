using Database.Initialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Ostrovskyi.UI.Data;
using Ostrovskyi.UI.Entities;
using Ostrovskyi.UI.Services;
using Serilog;
using System.Security.Claims;

internal class Program
{
 
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", p =>
                p.RequireClaim(ClaimTypes.Role, "admin"));
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(opt =>
            {
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddHttpClient<IProductService, ApiProductService>(opt
                => opt.BaseAddress = new Uri("https://localhost:7002/api/kosuhi/"));

            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt
            => opt.BaseAddress = new
            Uri("https://localhost:7002/api/categories/"));


            //builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            //builder.Services.AddScoped<IProductService,MemoryProductService>();
            builder.Services.AddHttpContextAccessor();

        //builder.Services.AddScoped<Cart>(sp => CorsService.GetCart(sp));

        
        builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddSerilog();
            builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();
        builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
            });

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddFilter("Microsoft", LogLevel.None);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();




            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval:
            RollingInterval.Day)
            .CreateLogger();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            var serviceProvider = app.Services.CreateScope().ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //logger.AddFile("Logs/log-{Date}.txt");
            //DbInitializer.Seed(context, userManager, roleManager).Wait();
            //app.UseFileLogging();

            app.Run();
        }
    }
