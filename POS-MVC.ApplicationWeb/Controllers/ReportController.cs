using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class ReportController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISaleService _saleService;

        public ReportController(IMapper mapper, ISaleService saleService)
        {
            _mapper = mapper;
            _saleService = saleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReportSale(string startDate, string endDate)
        {
            List<ReportSaleViewModel> reportSaleViewModel = _mapper.Map<List<ReportSaleViewModel>>(await _saleService.ReportSale(startDate, endDate));
            return StatusCode(StatusCodes.Status200OK, new { data = reportSaleViewModel });

        }
    }
}
