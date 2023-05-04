using POS_MVC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_MVC.DAL.Interfaces
{
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        Task<Sale> Create(Sale entity);
        Task<List<SaleDetail>> Report(DateTime startDate, DateTime endDate);
    }
}
