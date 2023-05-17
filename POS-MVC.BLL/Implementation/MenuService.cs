using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.BLL.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Menu> _repositoryMenu;
        private readonly IGenericRepository<RoleMenu> _repositoryRoleMenu;
        private readonly IGenericRepository<User> _repositoryUser;

        public MenuService(IGenericRepository<Menu> genericRepository, IGenericRepository<RoleMenu> repositoryRoleMenu, IGenericRepository<User> repositoryUser)
        {
            _repositoryMenu = genericRepository;
            _repositoryRoleMenu = repositoryRoleMenu;
            _repositoryUser = repositoryUser;
        }

        public async Task<List<Menu>> GetMenuListAsync(int userId)
        {
            IQueryable<User> user = await _repositoryUser.SearchAsync(u => u.UserId == userId);
            IQueryable<RoleMenu> roleMenu = await _repositoryRoleMenu.SearchAsync();
            IQueryable<Menu> menu = await _repositoryMenu.SearchAsync();

            IQueryable<Menu> parentMenu = (from u in user
                                           join rm in roleMenu on u.RoleId equals rm.RoleId
                                           join m in menu on rm.MenuId equals m.MenuId
                                           join parentm in menu on m.MenuId equals parentm.MenuId
                                           select parentm).Distinct().AsQueryable();

            IQueryable<Menu> childsMenu = (from u in user
                                           join rm in roleMenu on u.RoleId equals rm.RoleId
                                           join m in menu on rm.MenuId equals m.MenuId
                                           where m.MenuId != m.ParentMenuId
                                           select m).Distinct().AsQueryable();

            List<Menu> menus = (from parentm in parentMenu
                                select new Menu()
                                {
                                    Description = parentm.Description,
                                    Icon = parentm.Icon,
                                    Controller = parentm.Controller,
                                    PageAction = parentm.PageAction,
                                    InverseParentMenu = (from childm in childsMenu
                                                         where childm.ParentMenuId == parentm.MenuId
                                                         select childm).ToList()

                                }).ToList();
            return menus;
        }
    }
}
