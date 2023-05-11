using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface ISaleService
    {
        Task<List<Product>> GetProduct(string search);
        Task<Sale> CreateSale(Sale entity);
        Task<List<Sale>> RecordSales(string saleNumber, string startDate, string endDate);
        Task<Sale> DetailSale(string saleNumber);
        Task<List<SaleDetail>> ReportSale(string startDate, string endDate);
    }
}
