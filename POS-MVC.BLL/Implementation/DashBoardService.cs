using Microsoft.EntityFrameworkCore;
using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;
using System.Globalization;

namespace POS_MVC.BLL.Implementation
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IGenericRepository<SaleDetail> _saleDetailRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private DateTime _startDate = DateTime.Now;

        public DashBoardService(ISaleRepository saleRepository,
                                IGenericRepository<SaleDetail> saleDetailRepository,
                                IGenericRepository<Category> categoryRepository,
                                IGenericRepository<Product> productRepository)
        {
            _saleRepository = saleRepository;
            _saleDetailRepository = saleDetailRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _startDate = _startDate.AddDays(-7);
        }

        public async Task<int> TotalSaleLastWeek()
        {
            try
            {
                IQueryable<Sale> query = await _saleRepository.SearchAsync(s => s.CreationDate >= _startDate.Date);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> TotalIncomesLastWeek()
        {
            try
            {
                IQueryable<Sale> query = await _saleRepository.SearchAsync(s => s.CreationDate.Value.Date >= _startDate.Date);
                decimal result = query.Select(s => s.Total).Sum(s => s.Value);
                return Convert.ToString(result, new CultureInfo("es-CO"));
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TotalProducts()
        {
            try
            {
                IQueryable<Product> query = await _productRepository.SearchAsync();
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TotalCategories()
        {
            try
            {
                IQueryable<Category> query = await _categoryRepository.SearchAsync();
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> SaleLastWeek()
        {
            try
            {
                IQueryable<Sale> query = await _saleRepository.SearchAsync(s => s.CreationDate.Value.Date >= _startDate.Date);

                Dictionary<string, int> result = query
                    .GroupBy(s => s.CreationDate!.Value.Date).OrderByDescending(g => g.Key)
                    .Select(sd => new { date = sd.Key.ToString("dd/MM/yyyy"), total = sd.Count() })
                    .ToDictionary(keySelector: r => r.date, elementSelector: r => r.total);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> ProductsTopLastWeek()
        {
            try
            {
                IQueryable<SaleDetail> query = await _saleDetailRepository.SearchAsync();

                Dictionary<string, int> result = query
                    .Include(s => s.Sale)
                    .Where(sd => sd.Sale!.CreationDate!.Value.Date >= _startDate.Date)
                    .GroupBy(sd => sd.ProductDescription).OrderByDescending(g => g.Count())
                    .Select(sd => new { product = sd.Key, total = sd.Count() }).Take(4)
                    .ToDictionary(keySelector: r => r.product!, elementSelector: r => r.total);

                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}
