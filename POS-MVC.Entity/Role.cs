using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class Role
    {
        public Role()
        {
            RoleMenus = new HashSet<RoleMenu>();
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
