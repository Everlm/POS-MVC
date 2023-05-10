using Microsoft.EntityFrameworkCore;
using POS_MVC.BLL.Interfaces;
using POS_MVC.DAL.Interfaces;
using POS_MVC.Entity;

namespace POS_MVC.BLL.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IFireBaseService _fireBaseService;


        public ProductService(IGenericRepository<Product> repository, IFireBaseService fireBaseService)
        {
            _repository = repository;
            _fireBaseService = fireBaseService;

        }

        public async Task<List<Product>> ListProducts()
        {
            IQueryable<Product> query = await _repository.SearchAsync();
            return query.Include(c => c.Category).ToList();
        }

        public async Task<Product> CreateProduct(Product entity, Stream image = null, string nameImage = "")
        {
            Product productFound = await _repository.GetAsync(p => p.BarCode == entity.BarCode);
            if (productFound != null)
            {
                throw new TaskCanceledException("Bar code exist");
            }

            try
            {
                entity.ImageName = nameImage;
                if (image != null)
                {
                    string urlImage = await _fireBaseService.UploadStorageAsync(image, "product_folder", nameImage);
                    entity.ImagenUrl = urlImage;
                }

                Product product = await _repository.CreateAsync(entity);

                if (product.ProductId == 0)
                {
                    throw new TaskCanceledException("Failed to create product");
                }

                IQueryable<Product> query = await _repository.SearchAsync(p => p.ProductId == product.ProductId);
                product = query.Include(c => c.Category).First();

                return product;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Product> UpdateProduct(Product entity, Stream image = null, string nameImage = "")
        {
            Product productFound = await _repository.GetAsync(p => p.BarCode == entity.BarCode && p.ProductId != entity.ProductId);

            if (productFound != null)
            {
                throw new TaskCanceledException("Bar code exist");
            }

            try
            {
                IQueryable<Product> query = await _repository.SearchAsync(p => p.ProductId == entity.ProductId);

                Product product = query.First();
                product.BarCode = entity.BarCode;
                product.Brand = entity.Brand;
                product.Description = entity.Description;
                product.CategoryId = entity.CategoryId;
                product.Stock = entity.Stock;
                product.Price = entity.Price;
                product.IsActive = entity.IsActive;

                if (product.ImageName == "")
                {
                    product.ImageName = nameImage;
                }

                if (image != null)
                {
                    string urlImage = await _fireBaseService.UploadStorageAsync(image, "product_folder", product.ImageName);
                    product.ImagenUrl = urlImage;
                }

                bool response = await _repository.UpdateAsync(product);
                if (!response)
                    throw new TaskCanceledException("Failed to edit product");

                Product productEdit = query.Include(c => c.Category).First();
                return productEdit;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product productFound = await _repository.GetAsync(p => p.ProductId == productId);
                if (productFound == null)
                {
                    throw new TaskCanceledException("Product not exist");
                }

                string nameImage = productFound.ImageName;
                bool response = await _repository.DeleteAsync(productFound);

                if (response)
                {
                    await _fireBaseService.DeleteStorageAsync("product_folder", nameImage);
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

    }
}
