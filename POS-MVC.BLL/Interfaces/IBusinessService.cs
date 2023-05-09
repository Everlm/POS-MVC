using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface IBusinessService
    {
        Task<Business> GetAsync();
        Task<Business> SaveAsync(Business entity, Stream Logo = null, string LogoName = "");
    }
}
