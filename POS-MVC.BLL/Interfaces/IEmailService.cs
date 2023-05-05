namespace POS_MVC.BLL.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string destinationEmail, string subject, string message);
    }
}
