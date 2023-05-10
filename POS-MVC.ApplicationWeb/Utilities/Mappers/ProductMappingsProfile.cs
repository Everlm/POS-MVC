using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;
using System.Globalization;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class ProductMappingsProfile : Profile
    {
        public ProductMappingsProfile()
        {
            CreateMap<Product, ProductViewModel>()
                  .ForMember(x => x.IsActive, x => x.MapFrom(x => x.IsActive == true ? 1 : 0))
                  .ForMember(x => x.Category, x => x.MapFrom(x => x.Category.Description))
                  .ForMember(x => x.Price, x => x.MapFrom(x => Convert.ToString(x.Price.Value, new CultureInfo("es-CO"))));

            CreateMap<ProductViewModel, Product>()
                 .ForMember(x => x.IsActive, x => x.MapFrom(x => x.IsActive == 1 ? true : false))
                 .ForMember(x => x.Category, x => x.Ignore())
                 .ForMember(x => x.Price, x => x.MapFrom(x => Convert.ToDecimal(x.Price, new CultureInfo("es-CO"))));
        }
    }
}
