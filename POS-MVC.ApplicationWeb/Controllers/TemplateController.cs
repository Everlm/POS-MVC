using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class TemplateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;
        private readonly ISaleService _saleService;

        public TemplateController(IMapper mapper, IBusinessService businessService, ISaleService saleService)
        {
            _mapper = mapper;
            _businessService = businessService;
            _saleService = saleService;
        }

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

        public async Task<IActionResult> SalePDF(string saleNumber)
        {
            SaleViewModel saleViewModel = _mapper.Map<SaleViewModel>(await _saleService.DetailSale(saleNumber));
            BusinessViewModel businessViewModel = _mapper.Map<BusinessViewModel>(await _businessService.GetAsync());

            SalePDFViewModel salePDFViewModel = new SalePDFViewModel();
            salePDFViewModel.Business = businessViewModel;
            salePDFViewModel.Sale = saleViewModel;

            return View(salePDFViewModel);
        }
    }
}
