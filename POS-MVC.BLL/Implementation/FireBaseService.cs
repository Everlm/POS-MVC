using Firebase.Auth;
using Firebase.Storage;
using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.BLL.Implementation
{
    public class FireBaseService : IFireBaseService
    {
        private readonly IGenericRepository<Configuration> _configurationRepository;

        public FireBaseService(IGenericRepository<Configuration> configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<string> UploadStorageAsync(Stream FileStream, string DestinationFolder, string FileName)
        {
            string ImageUrl = "";
            try
            {
                IQueryable<Configuration> query = await _configurationRepository.SearchAsync(c => c.Resource.Equals("Firebase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Property, elementSelector: c => c.Value);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["password"]);

                var cancellationToken = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["route"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(Config[DestinationFolder])
                    .Child(FileName)
                    .PutAsync(FileStream, cancellationToken.Token);

                ImageUrl = await task;

            }
            catch (Exception ex)
            {
                ImageUrl = "";
                throw new Exception($"Error in StorageService, {ex.Message}");
            }

            return ImageUrl;
        }

        public async Task<bool> DeleteStorageAsync(string DestinationFolder, string FileName)
        {
            try
            {
                IQueryable<Configuration> query = await _configurationRepository.SearchAsync(c => c.Resource.Equals("Firebase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Property, elementSelector: c => c.Value);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["password"]);

                var cancellationToken = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["route"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(Config[DestinationFolder])
                    .Child(FileName)
                   .DeleteAsync();

                await task;
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception($"Error in DeleteStorageService, {ex.Message}");
            }
        }

    }
}
