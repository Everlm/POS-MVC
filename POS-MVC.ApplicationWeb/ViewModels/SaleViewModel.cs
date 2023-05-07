using POS_MVC.Entity;

namespace POS_MVC.ApplicationWeb.ViewModels
{
    public class SaleViewModel
    {
        public int SaleId { get; set; }
        public string? SaleNumber { get; set; }
        public int? SalesDocumentTypeId { get; set; }
        public string? SalesDocumentType { get; set; }
        public int? UserId { get; set; }
        public string? User { get; set; }
        public string? CustomerDocument { get; set; }
        public string? CustomerName { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Total { get; set; }
        public string? CreationDate { get; set; }
        public virtual ICollection<SaleDetailViewModel> SaleDetails { get; set; }
    }
}
