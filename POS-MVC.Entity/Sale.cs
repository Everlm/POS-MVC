using System;
using System.Collections.Generic;

namespace POS_MVC.Entity
{
    public partial class Sale
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
        }

        public int SaleId { get; set; }
        public string? SaleNumber { get; set; }
        public int? SalesDocumentTypeId { get; set; }
        public int? UserId { get; set; }
        public string? CustomerDocument { get; set; }
        public string? CustomerName { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Total { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual SalesDocumentType? SalesDocumentType { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
