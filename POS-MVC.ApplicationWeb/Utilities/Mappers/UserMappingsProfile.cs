using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class UserMappingsProfile : Profile
    {
        public UserMappingsProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.IsActive, x => x.MapFrom(x => x.IsActive == true ? 1 : 0))
                .ForMember(x => x.Role, x => x.MapFrom(x => x.Role!.Description));

            CreateMap<UserViewModel, User>()
                  .ForMember(x => x.IsActive, x => x.MapFrom(x => x.IsActive == 1 ? true : false))
                  .ForMember(x => x.Role, x => x.Ignore());

        }
    }
}
