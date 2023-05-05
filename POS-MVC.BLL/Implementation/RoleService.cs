using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_MVC.BLL.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _genericRepository;

        public RoleService(IGenericRepository<Role> genericRepository)
        {
            this._genericRepository = genericRepository;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            IQueryable<Role> query = await _genericRepository.SearchAsync();
            return query.ToList();
        }
    }
}
