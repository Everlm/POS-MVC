using POS_MVC.BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;


namespace POS_MVC.BLL.Implementation
{
    public class UtilitiesService : IUtilitiesService
    {

        public string GeneratePassword()
        {
            string password = Guid.NewGuid().ToString("N").Substring(0, 6);
            return password;
        }

        public string ConverterSha256(string text)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }

    }
}
