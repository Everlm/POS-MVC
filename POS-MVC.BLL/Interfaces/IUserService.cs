using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByCredentialsAsync(string email, string password);
        Task<User> CreateAsync(User entity, Stream Photo = null!, string NamePhoto = "", string TemplateEmailUrl = "");
        Task<bool> SaveProfileAsync(User entity);
        Task<User> EditAsync(User entity, Stream Photo = null!, string NamePhoto = "");     
        Task<bool> DeleteAsync(int id);     
        Task<bool> ChangePasswordAsync(int id, string CurrentPassword, string NewPassword);
        Task<bool> ResetPasswordAsync(string email, string TemplateEmailUrl);
    }
}
