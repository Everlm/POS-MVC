using POS_MVC.Entity;

namespace POS_MVC.BLL.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> ListProducts();
        Task<Product> CreateProduct(Product entity, Stream image = null, string nameImage = "");
        Task<Product> UpdateProduct(Product entity, Stream image = null);
        Task<bool> DeleteProduct(int productId);
    }
}
