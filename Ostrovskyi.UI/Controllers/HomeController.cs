using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ostrovskyi.UI.Models;
using System.Diagnostics;

namespace Ostrovskyi.UI.Controllers
{
    public class HomeController : Controller
    {
        
            private readonly ILogger<HomeController> _logger;

            List<ListDemo> listDemos;
            public HomeController(ILogger<HomeController> logger)
            {
                _logger = logger;
                listDemos = new List<ListDemo>()
            {
                new ListDemo {Id = 1, Name="Item 1"},
                new ListDemo {Id = 2, Name = "Item 2"}
            };
            }



            public IActionResult Index()
            {
                //Log.Information("Hello из метода Index контроллера Home!");
                ViewData["Demo"] = new SelectList(listDemos, "Id", "Name");
                return View();

            }

            public IActionResult Privacy()
            {

                return View();
            }

            public IActionResult Product()
            {

                return View();
            }


            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            public class ListDemo
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }

        }
    }

