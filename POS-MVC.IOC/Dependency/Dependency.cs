using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS_MVC.BLL.Implementation;
using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.DBContext;
using POS_MVC.DAL.Implementation;
using POS_MVC.DAL.Interfaces;

namespace POS_MVC.IOC.Dependency
{
    public static class Dependency
    {
        public static void DependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFireBaseService, FireBaseService>();
            services.AddScoped<IUtilitiesService, UtilitiesService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBusinessService, BusinessService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
