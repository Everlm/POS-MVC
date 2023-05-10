using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.BLL.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;

        public CategoryService(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> ListCategories()
        {
            IQueryable<Category> query = await _repository.SearchAsync();
            return query.ToList();
        }
        public async Task<Category> CreateCategory(Category entity)
        {
            try
            {
                Category category = await _repository.CreateAsync(entity);
                if (category.CategoryId == 0)
                {
                    throw new TaskCanceledException("Error Create Category");
                }

                return category;
            }
            catch
            {

                throw;
            }
        }
        public async Task<Category> UpdateCategory(Category entity)
        {
            try
            {
                Category category = await _repository.GetAsync(c => c.CategoryId == entity.CategoryId);
                category.Description = entity.Description;
                category.IsActive = entity.IsActive;

                bool response = await _repository.UpdateAsync(category);

                if (!response)
                {
                    throw new TaskCanceledException("Error Update Category");
                }

                return category;
            }
            catch
            {

                throw;
            }
        }
        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                Category category = await _repository.GetAsync(c => c.CategoryId == categoryId);

                if (category == null)
                {
                    throw new TaskCanceledException("Category no exist");
                }

                bool response = await _repository.DeleteAsync(category);
                return response;    
            }
            catch
            {

                throw;
            }
        }
    }
}
