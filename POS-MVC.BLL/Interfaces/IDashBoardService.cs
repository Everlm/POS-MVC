namespace POS_MVC.BLL.Interfaces
{
    public interface IDashBoardService
    {
        Task<int> TotalSaleLastWeek();
        Task<string> TotalIncomesLastWeek();
        Task<int> TotalProducts();
        Task<int> TotalCategories();
        Task<Dictionary<string, int>> SaleLastWeek();
        Task<Dictionary<string, int>> ProductsTopLastWeek();
    }
}
