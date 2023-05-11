using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.BLL.Implementation
{
    public class SaleDocumentTypeService : ISaleDocumentTypeService
    {
        private readonly IGenericRepository<SalesDocumentType> _repository;

        public SaleDocumentTypeService(IGenericRepository<SalesDocumentType> repository)
        {
            _repository = repository;
        }

        public async Task<List<SalesDocumentType>> ListSaleDocumentType()
        {
            IQueryable<SalesDocumentType> query = await _repository.SearchAsync();
            return query.ToList();
        }
    }
}
