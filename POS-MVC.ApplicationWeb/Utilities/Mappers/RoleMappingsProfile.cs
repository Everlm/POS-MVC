using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class RoleMappingsProfile : Profile
    {
        public RoleMappingsProfile()
        {
            CreateMap<Role, RoleViewModel>();
        }
    }
}
