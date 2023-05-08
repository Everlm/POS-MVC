using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;
using System.Net;
using System.Net.Mail;

namespace POS_MVC.BLL.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IGenericRepository<Configuration> _configurationRepository;

        public EmailService(IGenericRepository<Configuration> configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<bool> SendEmailAsync(string destinationEmail, string subject, string message)
        {
            try
            {
                IQueryable<Configuration> query = await _configurationRepository.SearchAsync(c => c.Resource.Equals("Email_Service"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Property, elementSelector: c => c.Value);

                var credentials = new NetworkCredential(Config["email"], Config["password"]);

                var email = new MailMessage()
                {
                    From = new MailAddress(Config["email"], Config["alias"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true

                };

                email.To.Add(new MailAddress(destinationEmail));

                var serverClient = new SmtpClient()
                {
                    Host = Config["host"],
                    Port = int.Parse(Config["port"]),
                    Credentials = credentials,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                serverClient.Send(email);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in EmailService, {ex.Message}");
            }
        }
    }
}
