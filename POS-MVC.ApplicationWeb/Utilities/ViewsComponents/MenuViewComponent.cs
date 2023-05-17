using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.BLL.Interfaces;
using System.Security.Claims;

namespace POS_MVC.ApplicationWeb.Utilities.ViewsComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuViewComponent(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            List<MenuViewModel> listMenu;

            if (claimsUser.Identity.IsAuthenticated)
            {
                string? userId = claimsUser.Claims
                   .Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();

                listMenu = _mapper.Map<List<MenuViewModel>>(await _menuService.GetMenuListAsync(int.Parse(userId)));
            }
            else
            {
                listMenu = new List<MenuViewModel> { };
            }

            return View(listMenu);
        }
    }
}
