using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;
using Microsoft.Extensions.Logging;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _Logger;

        [AllowAnonymous]
        public async Task<ActionResult> IsNameFree(string UserName) 
        {
            var user = await _userManager.FindByNameAsync(UserName);
            return Json(user is null ? "true" : "Пользователь с таким именем уже существует");
        }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> Logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Logger = Logger;
        }

      //  [HttpGet]
      //  [Route("login/{returnUrl?}")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => View(new LoginViewModel { ReturnUrl = returnUrl });       

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
         //  var returnurl = Request.Headers["Referer"].ToString();
         //   model.ReturnUrl = returnurl;

            if (ModelState.IsValid)
            {         
                var loginResult = await _signInManager.PasswordSignInAsync
                    (model.UserName,
                    model.Password,
                    model.RememberMe,
#if DEBUG
                false
#else
                true
# endif
                );

                if (loginResult.Succeeded)
                {
                    _Logger.LogInformation("Успешный вход в систему {0}", model.UserName);

                    return LocalRedirect(model.ReturnUrl ?? "/");
                }        
            }

            ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле");
            return View(model);
        }

       // [HttpGet]
       [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User { UserName = model.UserName, Email = model.Email };
            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (createResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _userManager.AddToRoleAsync(user, "User");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var identityError in createResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }

                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            var user_name = User.Identity!.IsAuthenticated ? User.Identity.Name : "anonymous";

            return View();
        }
    }
}
