using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POS_MVC.ApplicationWeb.Utilities.Response;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Implementation;
using POS_MVC.BLL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListProducts()
        {
            List<ProductViewModel> productViewModel = _mapper.Map<List<ProductViewModel>>(await _productService.ListProducts());

            return StatusCode(StatusCodes.Status200OK, new { data = productViewModel });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] IFormFile image, [FromForm] string productModel)
        {
            GenericResponse<ProductViewModel> response = new GenericResponse<ProductViewModel>();

            try
            {
                ProductViewModel productViewModel = JsonConvert.DeserializeObject<ProductViewModel>(productModel);

                string nameImage = "";
                Stream streamImage = null;

                if (image != null)
                {
                    string nameCode = Guid.NewGuid().ToString("N");
                    string imageExtension = Path.GetExtension(image.FileName);
                    nameImage = string.Concat(nameCode, imageExtension);
                    streamImage = image.OpenReadStream();
                }

                Product productCreated = await _productService.CreateProduct(_mapper.Map<Product>(productViewModel), streamImage, nameImage);

                productViewModel = _mapper.Map<ProductViewModel>(productCreated);

                response.State = true;
                response.Object = productViewModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] IFormFile image, [FromForm] string productModel)
        {
            GenericResponse<ProductViewModel> response = new GenericResponse<ProductViewModel>();

            try
            {
                ProductViewModel productViewModel = JsonConvert.DeserializeObject<ProductViewModel>(productModel);

                string nameImage = "";
                Stream streamImage = null;

                if (image != null)
                {
                    string nameCode = Guid.NewGuid().ToString("N");
                    string imageExtension = Path.GetExtension(image.FileName);
                    nameImage = string.Concat(nameCode, imageExtension);
                    streamImage = image.OpenReadStream();
                }

                Product productUpdated = await _productService.UpdateProduct(_mapper.Map<Product>(productViewModel), streamImage, nameImage);

                productViewModel = _mapper.Map<ProductViewModel>(productUpdated);

                response.State = true;
                response.Object = productViewModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                response.State = await _productService.DeleteProduct(productId);

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
