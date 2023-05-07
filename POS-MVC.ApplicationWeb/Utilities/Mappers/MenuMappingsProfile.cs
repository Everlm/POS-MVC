using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class MenuMappingsProfile : Profile
    {
        public MenuMappingsProfile()
        {
            CreateMap<Menu, MenuViewModel>()
                .ForMember(x => x.InverseParentMenu, x => x.MapFrom(x => x.InverseParentMenu));
        }
    }
}
