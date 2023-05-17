using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetMenuListAsync(int userId);
    }
}
