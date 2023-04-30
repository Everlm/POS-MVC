using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class Menu
    {
        public Menu()
        {
            InverseParentMenu = new HashSet<Menu>();
            RoleMenus = new HashSet<RoleMenu>();
        }

        public int MenuId { get; set; }
        public string? Description { get; set; }
        public int? ParentMenuId { get; set; }
        public string? Icon { get; set; }
        public string? Controller { get; set; }
        public string? PageAction { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual Menu? ParentMenu { get; set; }
        public virtual ICollection<Menu> InverseParentMenu { get; set; }
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
