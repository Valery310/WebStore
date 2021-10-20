﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IValuesService _valuesService;

        public HomeController(ILogger<HomeController> logger, IValuesService valuesService)
        {
            _logger = logger;
            _valuesService = valuesService;
        }

        public async Task<IActionResult> Index()
        {
            throw new InvalidOperationException();

            _logger.LogInformation("Start : Open Index view");

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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                return RedirectToAction("Error");
            return Content($"Статуcный код ошибки: {id}");
        }


    }
}
