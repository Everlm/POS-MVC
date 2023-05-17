using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POS_MVC.ApplicationWeb.Utilities.Response;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Controllers
{
    [Authorize]
    public class BusinessController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;

        public BusinessController(IMapper mapper, IBusinessService businessService)
        {
            _mapper = mapper;
            _businessService = businessService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBusiness()
        {
            GenericResponse<BusinessViewModel> response = new GenericResponse<BusinessViewModel>();

            try
            {
                BusinessViewModel businessViewModel = _mapper.Map<BusinessViewModel>(await _businessService.GetAsync());

                response.State = true;
                response.Object = businessViewModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBusiness([FromForm] IFormFile logo, [FromForm] string model)
        {
            GenericResponse<BusinessViewModel> response = new GenericResponse<BusinessViewModel>();

            try
            {
                BusinessViewModel businessViewModel = JsonConvert.DeserializeObject<BusinessViewModel>(model);

                string logoName = "";
                Stream logoStream = null;

                if (logo != null)
                {
                    string nameCode = Guid.NewGuid().ToString("N");
                    string imageExtension = Path.GetExtension(logo.FileName);
                    logoName = string.Concat(nameCode, imageExtension);
                    logoStream = logo.OpenReadStream();
                }

                Business businessEdit = await _businessService.SaveAsync(_mapper.Map<Business>(businessViewModel), logoStream, logoName);

                businessViewModel = _mapper.Map<BusinessViewModel>(businessEdit);

                response.State = true;
                response.Object = businessViewModel;
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
