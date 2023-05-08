using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POS_MVC.ApplicationWeb.Utilities.Response;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IMapper mapper, IUserService userService, IRoleService roleService)
        {
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            List<RoleViewModel> roles = _mapper.Map<List<RoleViewModel>>(await _roleService.GetAllAsync());
            return StatusCode(StatusCodes.Status200OK, roles);
        }

        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            List<UserViewModel> users = _mapper.Map<List<UserViewModel>>(await _userService.GetAllAsync());
            return StatusCode(StatusCodes.Status200OK, new { data = users });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] IFormFile photo, [FromForm] string model)
        {
            GenericResponse<UserViewModel> response = new GenericResponse<UserViewModel>();

            try
            {
                UserViewModel userViewModel = JsonConvert.DeserializeObject<UserViewModel>(model);

                string photoName = "";
                Stream photoStream = null;

                if (photo != null)
                {
                    string nameCode = Guid.NewGuid().ToString("N");
                    string imageExtension = Path.GetExtension(photo.FileName);
                    photoName = string.Concat(nameCode, imageExtension);
                    photoStream = photo.OpenReadStream();
                }

                string urlTemplateEmail = $"{this.Request.Scheme}://{this.Request.Host}/Template/SendPassword?email=[email]&password=[password]";

                User userCreated = await _userService.CreateAsync(_mapper.Map<User>(userViewModel), photoStream, photoName, urlTemplateEmail);

                userViewModel = _mapper.Map<UserViewModel>(userCreated);

                response.State = true;
                response.Object = userViewModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
                throw;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] IFormFile photo, [FromForm] string model)
        {
            GenericResponse<UserViewModel> response = new GenericResponse<UserViewModel>();

            try
            {
                UserViewModel userViewModel = JsonConvert.DeserializeObject<UserViewModel>(model);

                string photoName = "";
                Stream photoStream = null;

                if (photo != null)
                {
                    string nameCode = Guid.NewGuid().ToString("N");
                    string imageExtension = Path.GetExtension(photo.FileName);
                    photoName = string.Concat(nameCode, imageExtension);
                    photoStream = photo.OpenReadStream();
                }

                User userUpdated = await _userService.EditAsync(_mapper.Map<User>(userViewModel), photoStream, photoName);

                userViewModel = _mapper.Map<UserViewModel>(userUpdated);

                response.State = true;
                response.Object = userViewModel;
            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
                throw;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                response.State = await _userService.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
                throw;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

    }
}
