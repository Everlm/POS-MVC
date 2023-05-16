using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;
using POS_MVC.Entity;
using System.Security.Claims;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;

            if (claimsUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            User userFound = await _userService.GetByCredentialsAsync(model.Email, model.Password);

            if (userFound == null)
            {
                ViewData["Mensaje"] = "Por favor verifique sus credenciales";
                return View();
            }

            ViewData["Mensaje"] = null;

            List<Claim> claims = new List<Claim>()
            {
               new Claim(ClaimTypes.Name, userFound.Name),
               new Claim(ClaimTypes.NameIdentifier, userFound.UserId.ToString()),
               new Claim(ClaimTypes.Role, userFound.RoleId.ToString()),
               new Claim("PhotoUrl", userFound.PhotoUrl),
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), properties);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ResetPassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserLoginViewModel model)
        {
            try
            {
                string urlTemplateEmail = $"{this.Request.Scheme}://{this.Request.Host}/Template/ResetPassword?password=[password]";
                bool result = await _userService.ResetPasswordAsync(model.Email, urlTemplateEmail);

                if (result)
                {
                    ViewData["Mensaje"] = "Su contrasena fue restablecida. por favor revisar su correo";
                    ViewData["MensajeError"] = null;
                }
                else
                {
                    ViewData["MensajeError"] = "Hay un errror, intentelo de nuevo mas tarde o verifique su correo";
                    ViewData["Mensaje"] = null;
                }
            }
            catch (Exception ex)
            {
                ViewData["MensajeError"] = ex.Message;
                ViewData["Mensaje"] = null;
                throw;
            }

            return View();
        }
    }
}
