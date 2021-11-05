using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Filters;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IValuesService _valuesService;

        public HomeController(IValuesService valuesService, ILogger<HomeController> logger)
        {
            _logger = logger;
            _valuesService = valuesService;
        }

        public async Task<IActionResult> Index()
        {
           // throw new InvalidOperationException();

            _logger.LogInformation("Открытие {0} контроллера {1}", ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);

            var values = await _valuesService.GetAsync();
            return View(values);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string _RequestId = "";
            _RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError("Error page : {0}", _RequestId);
            return View(new ErrorViewModel { RequestId = _RequestId });
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult BlogSingle()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public new IActionResult NotFound()
        {
            return View();
        }

        [Route("/ErrorStatus/{id}")]
        public IActionResult ErrorStatus(string id)
        {
            if (id == "404")
            {
                 return RedirectToAction("Error");
            }
            return Content($"Статуcный код ошибки: {id}");

            //if (id == "404")
            //{
            //    return RedirectToAction("NotFound");
            //}
            //return RedirectToAction("Error", $"Статуcный код ошибки: {id}");
        }


    }
}
