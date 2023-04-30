using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class SalesDocumentType
    {
        public SalesDocumentType()
        {
            Sales = new HashSet<Sale>();
        }

        public int SalesDocumentTypeId { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
