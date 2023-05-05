using Microsoft.EntityFrameworkCore;
using POS_MVC.DAL.DBContext;
using POS_MVC.DAL.Interfaces;
using System.Linq.Expressions;

namespace POS_MVC.DAL.Implementation
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {

            try
            {
                TEntity entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filter);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetAsync, {ex.Message}");
            }
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CreateAsync, {ex.Message}");
            }
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in UpdateAsync, {ex.Message}");
            }
        }
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in DeleteAsync, {ex.Message}");
            }

        }

        public async Task<IQueryable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> queryEntity = filter == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(filter);
            return queryEntity.AsQueryable();
        }

    }
}
