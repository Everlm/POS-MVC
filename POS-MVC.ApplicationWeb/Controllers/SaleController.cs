using Microsoft.AspNetCore.Mvc;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class SaleController : Controller
    {
        public IActionResult NewSale()
        {
            return View();
        }

        public IActionResult SalesHistory()
        {
            return View();
        }
    }
}
