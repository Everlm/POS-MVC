using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface ISaleDocumentTypeService
    {
        Task<List<SalesDocumentType>> ListSaleDocumentType();
    }
}
