using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> ListCategories();
        Task<Category> CreateCategory(Category entity);
        Task<Category> UpdateCategory(Category entity);
        Task<bool> DeleteCategory(int categoryId);
    }
}
