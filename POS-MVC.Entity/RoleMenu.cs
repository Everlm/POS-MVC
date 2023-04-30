using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class RoleMenu
    {
        public int RoleMenuId { get; set; }
        public int? RoleId { get; set; }
        public int? MenuId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual Menu? Menu { get; set; }
        public virtual Role? Role { get; set; }
    }
}
