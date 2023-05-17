using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace POS_MVC.ApplicationWeb.Utilities.ViewsComponents
{
    public class UserMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;

            string userName = "";
            string photoUrl = "";

            if (claimsUser.Identity.IsAuthenticated)
            {
                userName = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                            .Select(c => c.Value).SingleOrDefault();

                photoUrl = ((ClaimsIdentity)claimsUser.Identity).FindFirst("PhotoUrl").Value;
            }

            ViewData["userName"] = userName;
            ViewData["photoUrl"] = photoUrl;

            return View();
        }
    }
}
