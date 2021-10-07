using Microsoft.AspNetCore.Mvc;
using WebStore.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain;
using Microsoft.AspNetCore.Authorization;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                var loginResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (loginResult.Succeeded)
                {
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
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
                RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var identityError in createResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
            }

            return View(new RegisterUserViewModel());
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterUserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // var user = new User { UserName = model.UserName, Email = model.Email, EmailConfirmed = true };              
        //        var user = new User { UserName = model.UserName, Email = model.Email };

        //        var createResult = await _userManager.CreateAsync(user, model.Password);

        //        if (createResult.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, false);
        //            RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            foreach (var identityError in createResult.Errors)
        //            {
        //                ModelState.AddModelError("", identityError.Description);
        //            }
        //        }
        //    }
        //    return View(model);
        //}

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
