using Microsoft.AspNetCore.Mvc;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class BusinessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
