using Microsoft.AspNetCore.Mvc;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
