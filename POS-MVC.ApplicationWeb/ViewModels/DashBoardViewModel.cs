namespace POS_MVC.ApplicationWeb.ViewModels
{
    public class DashBoardViewModel
    {
        public int TotalSales { get; set; }
        public string? TotalIncomes { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public List<SaleWeekViewModel> SaleWeek { get; set; }
        public List<ProductWeekViewModel> ProductWeek { get; set; }
    }
}
