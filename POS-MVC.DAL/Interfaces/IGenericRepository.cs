using System.Linq.Expressions;

namespace POS_MVC.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<IQueryable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> filter = null!);
    }

}
