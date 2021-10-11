using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValuesService _valuesService;

        public WebAPIController(IValuesService valuesService) => _valuesService = valuesService;

        public IActionResult Index()
        {
            var values = _valuesService.Get();
            return View(values);
        }
    }
}
