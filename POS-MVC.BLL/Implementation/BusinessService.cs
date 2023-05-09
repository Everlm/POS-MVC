using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.BLL.Implementation
{
    public class BusinessService : IBusinessService
    {
        private readonly IGenericRepository<Business> _repository;
        private readonly IFireBaseService _fireBaseService;

        public BusinessService(IGenericRepository<Business> repository, IFireBaseService fireBaseService)
        {
            _repository = repository;
            _fireBaseService = fireBaseService;
        }

        public async Task<Business> GetAsync()
        {
            try
            {
                Business business = await _repository.GetAsync(b => b.BusinessId == 1);
                return business;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Business> SaveAsync(Business entity, Stream Logo = null, string LogoName = "")
        {
            try
            {
                Business business = await _repository.GetAsync(b => b.BusinessId == 1);

                business.DocumentNumber = entity.DocumentNumber;
                business.Name = entity.Name;
                business.Email = entity.Email;
                business.Address = entity.Address;
                business.Phone = entity.Phone;
                business.TaxRate = entity.TaxRate;
                business.CurrencySymbol = entity.CurrencySymbol;

                business.LogoName = business.LogoName == "" ? LogoName : business.LogoName;

                if (Logo != null)
                {
                    string urlLogo = await _fireBaseService.UploadStorageAsync(Logo, "logo_folder", business.LogoName);
                    business.LogoUrl = urlLogo;
                }

                await _repository.UpdateAsync(business);
                return business;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in ResetPasswordUser, {ex.Message}");
            }
        }
    }
}
