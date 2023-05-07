namespace POS_MVC.ApplicationWeb.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string? BarCode { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? Category { get; set; }
        public int? Stock { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Price { get; set; }
        public int? IsActive { get; set; }
    }
}
