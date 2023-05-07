using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;
using System.Globalization;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class SaleMappingsProfile : Profile
    {
        public SaleMappingsProfile()
        {
            CreateMap<Sale, SaleViewModel>()
                 .ForMember(x => x.SalesDocumentType, x => x.MapFrom(x => x.SalesDocumentType.Description))
                 .ForMember(x => x.User, x => x.MapFrom(x => x.User.Name))
                 .ForMember(x => x.SubTotal, x => x.MapFrom(x => Convert.ToString(x.SubTotal.Value, new CultureInfo("es-CO"))))
                 .ForMember(x => x.TotalTax, x => x.MapFrom(x => Convert.ToString(x.TotalTax.Value, new CultureInfo("es-CO"))))
                 .ForMember(x => x.Total, x => x.MapFrom(x => Convert.ToString(x.Total.Value, new CultureInfo("es-CO"))))
                 .ForMember(x => x.CreationDate, x => x.MapFrom(x => x.CreationDate.Value.ToString("dd/MM/yyyy")));

            CreateMap<SaleViewModel, Sale>()
                .ForMember(x => x.SubTotal, x => x.MapFrom(x => Convert.ToDecimal(x.SubTotal, new CultureInfo("es-CO"))))
                .ForMember(x => x.TotalTax, x => x.MapFrom(x => Convert.ToDecimal(x.TotalTax, new CultureInfo("es-CO"))))
                .ForMember(x => x.Total, x => x.MapFrom(x => Convert.ToDecimal(x.Total, new CultureInfo("es-CO"))));
        }
    }
}
