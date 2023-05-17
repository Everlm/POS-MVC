using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.Utilities.Response;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListCategories()
        {
            List<CategoryViewModel> categoryViewModel = _mapper.Map<List<CategoryViewModel>>(await _categoryService.ListCategories());
            return StatusCode(StatusCodes.Status200OK, new { data = categoryViewModel });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryViewModel categoryModel)
        {
            GenericResponse<CategoryViewModel> response = new GenericResponse<CategoryViewModel>();

            try
            {
                Category category = await _categoryService.CreateCategory(_mapper.Map<Category>(categoryModel));
                categoryModel = _mapper.Map<CategoryViewModel>(category);

                response.State = true;
                response.Object = categoryModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;

            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryViewModel categoryModel)
        {
            GenericResponse<CategoryViewModel> response = new GenericResponse<CategoryViewModel>();

            try
            {
                Category category = await _categoryService.UpdateCategory(_mapper.Map<Category>(categoryModel));
                categoryModel = _mapper.Map<CategoryViewModel>(category);

                response.State = true;
                response.Object = categoryModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;

            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            GenericResponse<CategoryViewModel> response = new GenericResponse<CategoryViewModel>();

            try
            {
                response.State = await _categoryService.DeleteCategory(categoryId);

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
