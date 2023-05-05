using Microsoft.AspNetCore.Mvc;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class TemplateController : Controller
    {
        public IActionResult SendPassword(string email, string password)
        {
            ViewData["Email"] = email;
            ViewData["Password"] = password;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";

            return View();
        }
        public IActionResult ResetPassword(string password)
        {
            ViewData["Password"] = password;
            return View();
        }
    }
}
