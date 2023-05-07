using AutoMapper;
using POS_MVC.ApplicationWeb.ViewModels;
using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.Utilities.Mappers
{
    public class SaleDocumentTypeMappingsProfile : Profile
    {
        public SaleDocumentTypeMappingsProfile()
        {
            CreateMap<SalesDocumentType, SaleDocumentTypeViewModel>().ReverseMap();
        }
    }
}
