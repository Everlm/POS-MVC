using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class User
    {
        public User()
        {
            Sales = new HashSet<Sale>();
        }

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? PhotoName { get; set; }
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
