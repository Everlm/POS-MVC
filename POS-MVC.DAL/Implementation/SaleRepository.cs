using Microsoft.EntityFrameworkCore;
using POS_MVC.DAL.DBContext;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_MVC.DAL.Implementation
{
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Sale> Create(Sale entity)
        {
            Sale generatedSale = new Sale();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (SaleDetail item in entity.SaleDetails)
                    {
                        Product productFind = await _context.Products.Where(p => p.ProductId.Equals(item.ProductId)).FirstAsync();

                        productFind.Stock = productFind.Stock - item.Quantity;
                        _context.Products.Update(productFind);
                    }

                    await _context.SaveChangesAsync();

                    ConsecutiveNumber consecutive = _context.ConsecutiveNumbers.Where(n => n.TransactionType == "sale").First();
                    consecutive.LastNumber = consecutive.LastNumber + 1;
                    consecutive.UpdateDate = DateTime.Now;

                    _context.ConsecutiveNumbers.Update(consecutive);
                    await _context.SaveChangesAsync();

                    string zeros = string.Concat(Enumerable.Repeat("0", consecutive.DigitsNumber.Value));
                    string saleNumber = zeros + consecutive.LastNumber.ToString();
                    saleNumber = saleNumber.Substring(saleNumber.Length - consecutive.DigitsNumber.Value, consecutive.DigitsNumber.Value);

                    entity.SaleNumber = saleNumber;

                    await _context.Sales.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    generatedSale = entity;

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error in Create in Sale, {ex.Message}");
                }
            }
            return generatedSale;
        }

        public async Task<List<SaleDetail>> Report(DateTime startDate, DateTime endDate)
        {
            List<SaleDetail> resumeList = await _context.SaleDetails
                 .Include(s => s.Sale)
                 .ThenInclude(u => u.UserId)
                 .Include(s => s.Sale)
                 .ThenInclude(ds => ds.SalesDocumentType)
                 .Where(sd => sd.Sale.CreationDate.Value.Date >= startDate.Date &&
                 sd.Sale.CreationDate.Value.Date <= endDate.Date).ToListAsync();

            return resumeList;
        }
    }
}
