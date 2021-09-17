using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static readonly List<EmployeeView> _employees = new List<EmployeeView>
            {
                new EmployeeView(
                    1,
                    "Иван",
                    "Иванов",
                    "Иванович",
                    22)
                    
                ,
                new EmployeeView(
                    2,
                    "Владислав",
                    "Петров",
                    "Иванович",
                    35)
            };

        public IActionResult Index()
        {

            return View(_employees);
            //   return Content("Hello from controller!");
        }

        public IActionResult Details(int id)
        {
            return View(_employees[id - 1]);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
