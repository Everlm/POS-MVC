using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.Utilities.Response;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IDashBoardService _dashBoardService;

        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetResumen()
        {
            GenericResponse<DashBoardViewModel> response = new GenericResponse<DashBoardViewModel>();

            try
            {
                DashBoardViewModel dashBoardViewModel = new DashBoardViewModel();
                dashBoardViewModel.TotalSales = await _dashBoardService.TotalSaleLastWeek();
                dashBoardViewModel.TotalIncomes = await _dashBoardService.TotalIncomesLastWeek();
                dashBoardViewModel.TotalProducts = await _dashBoardService.TotalProducts();
                dashBoardViewModel.TotalCategories = await _dashBoardService.TotalCategories();

                List<SaleWeekViewModel> saleWeekViewModel = new List<SaleWeekViewModel>();
                List<ProductWeekViewModel> productWeekViewModel = new List<ProductWeekViewModel>();

                foreach (KeyValuePair<string, int> item in await _dashBoardService.SaleLastWeek())
                {
                    saleWeekViewModel.Add(new SaleWeekViewModel()
                    {
                        Date = item.Key,
                        Total = item.Value
                    });
                }

                foreach (KeyValuePair<string, int> item in await _dashBoardService.ProductsTopLastWeek())
                {
                    productWeekViewModel.Add(new ProductWeekViewModel()
                    {
                        Product = item.Key,
                        Quantity = item.Value
                    });
                }

                dashBoardViewModel.SaleWeek = saleWeekViewModel;
                dashBoardViewModel.ProductWeek = productWeekViewModel;

                response.State = true;
                response.Object = dashBoardViewModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
