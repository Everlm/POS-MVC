using Microsoft.AspNetCore.Mvc;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
