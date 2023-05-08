using Microsoft.EntityFrameworkCore;
using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;
using System.Net;
using System.Text;

namespace POS_MVC.BLL.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IFireBaseService _fireBaseService;
        private readonly IUtilitiesService _UtilitiesService;
        private readonly IEmailService _emailService;

        public UserService(IGenericRepository<User> genericRepository, IFireBaseService fireBaseService, IUtilitiesService utilitiesService, IEmailService emailService)
        {
            _genericRepository = genericRepository;
            _fireBaseService = fireBaseService;
            _UtilitiesService = utilitiesService;
            _emailService = emailService;
        }

        public async Task<List<User>> GetAllAsync()
        {
            IQueryable<User> query = await _genericRepository.SearchAsync();
            return await query.Include(r => r.Role).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            IQueryable<User> query = await _genericRepository.SearchAsync(u => u.UserId == id);
            User result = query.Include(r => r.Role).FirstOrDefault();

            return result;
        }

        public async Task<User> GetByCredentialsAsync(string email, string password)
        {
            string passwordEncrypted = _UtilitiesService.ConverterSha256(password);
            User userFound = await _genericRepository.GetAsync(u => u.Email.Equals(email) && u.Password.Equals(passwordEncrypted));

            return userFound;
        }

        public async Task<User> CreateAsync(User entity, Stream Photo = null, string NamePhoto = "", string TemplateEmailUrl = "")
        {
            User userExist = await _genericRepository.GetAsync(u => u.Email == entity.Email);

            if (userExist != null)
            {
                throw new TaskCanceledException("Email already exist");
            }

            try
            {
                string generatedPassword = _UtilitiesService.GeneratePassword();
                entity.Password = _UtilitiesService.ConverterSha256(generatedPassword);
                entity.PhotoName = NamePhoto;

                if (Photo != null)
                {
                    string photoUrl = await _fireBaseService.UploadStorageAsync(Photo, "user_folder", NamePhoto);
                    entity.PhotoUrl = photoUrl;
                }

                User userCreated = await _genericRepository.CreateAsync(entity);

                if (userCreated.UserId == 0)
                {
                    throw new TaskCanceledException("Error creating user");
                }

                if (TemplateEmailUrl != "")
                {
                    TemplateEmailUrl = TemplateEmailUrl.Replace("[email]", userCreated.Email).Replace("[password]", generatedPassword);

                    string emailHtml = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TemplateEmailUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader readerStream = null;

                            if (response.CharacterSet == null)
                            {
                                readerStream = new StreamReader(dataStream);
                            }
                            else
                            {
                                readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            emailHtml = readerStream.ReadToEnd();
                            response.Close();
                            readerStream.Close();
                        }
                    }

                    if (emailHtml != "")
                    {
                        await _emailService.SendEmailAsync(userCreated.Email, "Account Created", emailHtml);
                    }

                }

                IQueryable<User> query = await _genericRepository.SearchAsync(u => u.UserId == userCreated.UserId);
                userCreated = query.Include(r => r.Role).First();

                return userCreated;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CreateUser, {ex.Message}");
            }
        }

        public async Task<bool> SaveProfileAsync(User entity)
        {
            try
            {
                User userFound = await _genericRepository.GetAsync(u => u.UserId == entity.UserId);
                if (userFound == null)
                {
                    throw new TaskCanceledException("User not found");
                }

                userFound.Email = entity.Email;
                userFound.Phone = entity.Phone;

                bool response = await _genericRepository.UpdateAsync(userFound);
                return response;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in SaveProfileUser, {ex.Message}");
            }
        }

        public async Task<User> EditAsync(User entity, Stream Photo = null, string NamePhoto = "")
        {
            User userExist = await _genericRepository.GetAsync(u => u.Email == entity.Email && u.UserId != entity.UserId);

            if (userExist != null)
            {
                throw new TaskCanceledException("Email already exist");
            }

            try
            {
                IQueryable<User> queryUser = await _genericRepository.SearchAsync(u => u.UserId == entity.UserId);

                User userEdit = queryUser.First();
                userEdit.Name = entity.Name;
                userEdit.Email = entity.Email;
                userEdit.Phone = entity.Phone;
                userEdit.RoleId = entity.RoleId;
                userEdit.IsActive = entity.IsActive;

                if (userEdit.PhotoName == "")
                {
                    userEdit.PhotoName = NamePhoto;
                }
                if (Photo != null)
                {
                    string urlPhoto = await _fireBaseService.UploadStorageAsync(Photo, "user_folder", userEdit.PhotoName);
                    userEdit.PhotoUrl = urlPhoto;
                }

                bool response = await _genericRepository.UpdateAsync(userEdit);

                if (!response)
                {
                    throw new TaskCanceledException("Error editing user");
                }

                User userEdited = queryUser.Include(r => r.Role).First();
                return userEdited;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in UpdateUser, {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                User userFound = await _genericRepository.GetAsync(u => u.UserId == id);

                if (userFound == null)
                {
                    throw new TaskCanceledException("User not found");
                }

                string namePhoto = userFound.PhotoName;
                bool response = await _genericRepository.DeleteAsync(userFound);

                if (response)
                {
                    await _fireBaseService.DeleteStorageAsync("user_folder", namePhoto);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in DeleteUser, {ex.Message}");
            }
        }

        public async Task<bool> ChangePasswordAsync(int id, string CurrentPassword, string NewPassword)
        {
            try
            {
                User userFound = await _genericRepository.GetAsync(u => u.UserId == id);

                if (userFound == null)
                {
                    throw new TaskCanceledException("User not found");
                }

                if (userFound.Password != _UtilitiesService.ConverterSha256(CurrentPassword))
                {
                    throw new TaskCanceledException("The current password is incorrect");
                }

                userFound.Password = _UtilitiesService.ConverterSha256(NewPassword);
                bool response = await _genericRepository.UpdateAsync(userFound);

                return response;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in ChangePasswordUser, {ex.Message}");
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string TemplateEmailUrl)
        {
            try
            {
                User userFound = await _genericRepository.GetAsync(u => u.Email == email);

                if (userFound == null)
                {
                    throw new TaskCanceledException("User Email not found");
                }

                string generatedPassword = _UtilitiesService.GeneratePassword();
                userFound.Password = _UtilitiesService.ConverterSha256(generatedPassword);

                TemplateEmailUrl = TemplateEmailUrl.Replace("[password]", generatedPassword);

                string emailHtml = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TemplateEmailUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;

                        if (response.CharacterSet == null)
                        {
                            readerStream = new StreamReader(dataStream);
                        }
                        else
                        {
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        emailHtml = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();
                    }
                }
                bool emailsent = false;

                if (emailHtml != "")
                {
                    emailsent = await _emailService.SendEmailAsync(email, "Password Reset", emailHtml);
                }

                if (emailsent == false)
                {
                    throw new TaskCanceledException("Error sent email");
                }

                bool result = await _genericRepository.UpdateAsync(userFound);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in ResetPasswordUser, {ex.Message}");
            }
        }


    }
}
