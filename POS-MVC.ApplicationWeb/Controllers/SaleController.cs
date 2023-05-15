using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.Utilities.Response;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class SaleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISaleService _saleService;
        private readonly ISaleDocumentTypeService _saleDocumentTypeService;
        private readonly IConverter _converter;

        public SaleController(IMapper mapper, ISaleService saleService, ISaleDocumentTypeService saleDocumentTypeService, IConverter converter)
        {
            _mapper = mapper;
            _saleService = saleService;
            _saleDocumentTypeService = saleDocumentTypeService;
            _converter = converter;
        }

        public IActionResult NewSale()
        {
            return View();
        }

        public IActionResult SalesHistory()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListSaleDocumentType()
        {
            List<SaleDocumentTypeViewModel> saleDocumentTypeViewModel = _mapper.Map<List<SaleDocumentTypeViewModel>>(
                await _saleDocumentTypeService.ListSaleDocumentType());

            return StatusCode(StatusCodes.Status200OK, saleDocumentTypeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct(string search)
        {
            List<ProductViewModel> productViewModel = _mapper.Map<List<ProductViewModel>>(
                await _saleService.GetProduct(search));

            return StatusCode(StatusCodes.Status200OK, productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleViewModel model)
        {
            GenericResponse<SaleViewModel> response = new GenericResponse<SaleViewModel>();

            try
            {
                model.UserId = 3;

                Sale saleCreated = await _saleService.CreateSale(_mapper.Map<Sale>(model));
                model = _mapper.Map<SaleViewModel>(saleCreated);

                response.State = true;
                response.Object = model;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }


        [HttpGet]
        public async Task<IActionResult> RecordSales(string saleNumber, string startDate, string endDate)
        {
            List<SaleViewModel> saleViewModel = _mapper.Map<List<SaleViewModel>>(
                await _saleService.RecordSales(saleNumber, startDate, endDate));

            return StatusCode(StatusCodes.Status200OK, saleViewModel);
        }

        public IActionResult ShowPDF(string saleNumber)
        {
            string urlTemplate = $"{this.Request.Scheme}://{this.Request.Host}/Template/SalePDF?saleNumber={saleNumber}";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page = urlTemplate
                    }
                }
            };

            var filePDF = _converter.Convert(pdf);
            return File(filePDF, "application/pdf");
        }
    }
}
