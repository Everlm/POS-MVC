using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();
    }
}
