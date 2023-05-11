using Microsoft.EntityFrameworkCore;
using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;
using System.Globalization;

namespace POS_MVC.BLL.Implementation
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public SaleService(IGenericRepository<Product> productRepository, ISaleRepository saleRepository)
        {

            _productRepository = productRepository;
            _saleRepository = saleRepository;
        }

        public async Task<List<Product>> GetProduct(string search)
        {
            IQueryable<Product> query = await _productRepository.SearchAsync(p =>
                p.IsActive == true &&
                p.Stock > 0 &&
                string.Concat(p.BarCode, p.Brand, p.Description).Contains(search)
                );

            return query.Include(c => c.Category).ToList();
        }

        public async Task<Sale> CreateSale(Sale entity)
        {
            try
            {
                return await _saleRepository.Create(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Sale>> RecordSales(string saleNumber, string startDate, string endDate)
        {
            IQueryable<Sale> query = await _saleRepository.SearchAsync();
            startDate = startDate is null ? "" : startDate;
            endDate = endDate is null ? "" : endDate;

            if (startDate != "" && startDate != "")
            {
                DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-CO"));

                return query.Where(s =>
                     s.CreationDate!.Value.Date >= start_date.Date &&
                     s.CreationDate.Value.Date <= end_date.Date
                 )
                      .Include(sdt => sdt.SalesDocumentType)
                      .Include(u => u.User)
                      .Include(sd => sd.SaleDetails)
                      .ToList();
            }
            else
            {
                return query.Where(s =>
                    s.SaleNumber == saleNumber
                 )
                      .Include(sdt => sdt.SalesDocumentType)
                      .Include(u => u.User)
                      .Include(sd => sd.SaleDetails)
                      .ToList();
            }
        }

        public async Task<Sale> DetailSale(string saleNumber)
        {
            IQueryable<Sale> query = await _saleRepository.SearchAsync(s => s.SaleNumber == saleNumber);

            return query
                    .Include(sdt => sdt.SalesDocumentType)
                    .Include(u => u.User)
                    .Include(sd => sd.SaleDetails)
                    .First();
        }


        public async Task<List<SaleDetail>> ReportSale(string startDate, string endDate)
        {
            DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-CO"));
            DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-CO"));

            List<SaleDetail> saleDetails = await _saleRepository.Report(start_date, end_date);

            return saleDetails;
        }
    }
}
