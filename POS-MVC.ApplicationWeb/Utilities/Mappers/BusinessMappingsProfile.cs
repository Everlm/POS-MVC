using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;
using System.Globalization;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class BusinessMappingsProfile : Profile
    {
        public BusinessMappingsProfile()
        {
            CreateMap<Business, BusinessViewModel>()
                .ForMember(x => x.TaxRate, x => x.MapFrom(x => Convert.ToString(x.TaxRate.Value, new CultureInfo("es-CO"))));

            CreateMap<BusinessViewModel, Business>()
                .ForMember(x => x.TaxRate, x => x.MapFrom(x => Convert.ToDecimal(x.TaxRate, new CultureInfo("es-CO"))));
        }
    }
}
