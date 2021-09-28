using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult ProductDetails()
        {
            return View();

        }
    }
}
