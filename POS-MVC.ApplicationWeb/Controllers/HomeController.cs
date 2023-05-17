using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.Utilities.Response;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;
using POS_MVC.Entity;
using System.Security.Claims;

namespace POS_MVC.ApplicationWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            GenericResponse<UserViewModel> response = new GenericResponse<UserViewModel>();

            try
            {
                ClaimsPrincipal claimsUser = HttpContext.User;

                string? userId = claimsUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                UserViewModel user = _mapper.Map<UserViewModel>(await _userService.GetByIdAsync(int.Parse(userId)));

                response.State = true;
                response.Object = user;

            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
                
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile([FromBody] UserViewModel model)
        {
            GenericResponse<UserViewModel> response = new GenericResponse<UserViewModel>();

            try
            {
                ClaimsPrincipal claimsUser = HttpContext.User;

                string? userId = claimsUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                User user = _mapper.Map<User>(model);
                user.UserId = int.Parse(userId);

                bool result = await _userService.SaveProfileAsync(user);

                response.State = result;

            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
               
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            GenericResponse<bool> response = new GenericResponse<bool>();

            try
            {
                ClaimsPrincipal claimsUser = HttpContext.User;

                string? userId = claimsUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();


                bool result = await _userService.ChangePasswordAsync(int.Parse(userId), model.CurrentPassword, model.NewPassword);

                response.State = result;

            }
            catch (Exception ex)
            {
                response.State = false;
                response.Message = ex.Message;
               
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}