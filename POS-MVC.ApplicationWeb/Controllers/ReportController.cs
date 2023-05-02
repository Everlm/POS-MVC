using Microsoft.AspNetCore.Mvc;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
