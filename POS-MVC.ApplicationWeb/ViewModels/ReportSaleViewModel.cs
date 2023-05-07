namespace POS_MVC.ApplicationWeb.ViewModels
{
    public class ReportSaleViewModel
    {
        public string? CreationDate { get; set; }
        public string? SaleNumber { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentCustomer { get; set; }
        public string? NameCustomer { get; set; }
        public string? SubTotalSale { get; set; }
        public string? TotalTaxSale { get; set; }
        public string? TotalSale { get; set; }
        public string? Product { get; set; }
        public int Quantity { get; set; }
        public string? Price { get; set; }
        public string? Total { get; set; }
    }
}
