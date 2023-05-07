using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;
using System.Globalization;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class SaleDetailMappingsProfile : Profile
    {
        public SaleDetailMappingsProfile()
        {
            CreateMap<SaleDetail, SaleDetailViewModel>()
                 .ForMember(x => x.Price, x => x.MapFrom(x => Convert.ToString(x.Price.Value, new CultureInfo("es-CO"))))
                 .ForMember(x => x.Total, x => x.MapFrom(x => Convert.ToString(x.Total.Value, new CultureInfo("es-CO"))));

            CreateMap<SaleDetailViewModel, SaleDetail>()
                .ForMember(x => x.Price, x => x.MapFrom(x => Convert.ToDecimal(x.Price, new CultureInfo("es-CO"))))
                .ForMember(x => x.Total, x => x.MapFrom(x => Convert.ToDecimal(x.Total, new CultureInfo("es-CO"))));

            CreateMap<SaleDetail, ReportSaleViewModel>()
                .ForMember(x => x.CreationDate, x => x.MapFrom(x => x.Sale.CreationDate.Value.ToString("dd/MM/yyyy")))
                .ForMember(x => x.SaleNumber, x => x.MapFrom(x => x.Sale.SaleNumber))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => x.Sale.SalesDocumentType.Description))
                .ForMember(x => x.DocumentCustomer, x => x.MapFrom(x => x.Sale.CustomerDocument))
                .ForMember(x => x.NameCustomer, x => x.MapFrom(x => x.Sale.CustomerName))
                .ForMember(x => x.SubTotalSale, x => x.MapFrom(x => Convert.ToString(x.Sale.SubTotal.Value, new CultureInfo("es-CO"))))
                .ForMember(x => x.TotalTaxSale, x => x.MapFrom(x => Convert.ToString(x.Sale.TotalTax.Value, new CultureInfo("es-CO"))))
                .ForMember(x => x.TotalSale, x => x.MapFrom(x => Convert.ToString(x.Sale.Total.Value, new CultureInfo("es-CO"))))
                .ForMember(x => x.Price, x => x.MapFrom(x => Convert.ToString(x.Price.Value, new CultureInfo("es-CO"))))
                .ForMember(x => x.Total, x => x.MapFrom(x => Convert.ToString(x.Total.Value, new CultureInfo("es-CO"))))
                .ForMember(x => x.Product, x => x.MapFrom(x => x.ProductDescription));
        }
    }
}
