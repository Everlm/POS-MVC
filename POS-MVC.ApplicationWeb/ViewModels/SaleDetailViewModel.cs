namespace POS_MVC.ApplicationWeb.ViewModels
{
    public class SaleDetailViewModel
    {
        public int? ProductId { get; set; }
        public string? ProductBrand { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
        public int? Quantity { get; set; }
        public string? Price { get; set; }
        public string? Total { get; set; }
    }
}
