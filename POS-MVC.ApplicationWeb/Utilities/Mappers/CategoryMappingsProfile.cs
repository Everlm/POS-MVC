using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class CategoryMappingsProfile : Profile
    {
        public CategoryMappingsProfile()
        {
            CreateMap<Category, CategoryViewModel>()
               .ForMember(x => x.IsActive, x => x.MapFrom(x => x.IsActive == true ? 1 : 0));

            CreateMap<CategoryViewModel, Category>()
                   .ForMember(x => x.IsActive, x => x.MapFrom(x => x.IsActive == 1 ? true : false));
        }
    }
}
