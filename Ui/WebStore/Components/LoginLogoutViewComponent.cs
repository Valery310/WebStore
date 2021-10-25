using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components
{
    public class LoginLogoutViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
