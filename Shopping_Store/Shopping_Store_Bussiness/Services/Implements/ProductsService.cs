using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;
using Shopping_Store_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Bussiness.Services.Implements
{
	public class ProductsService : IProductsServices
	{
		private readonly IProductsRespository _productsRepostiory;

		public ProductsService(IProductsRespository productsServices)
		{
            _productsRepostiory = productsServices;
		}

		public async Task<List<Product>> GetAllProductAsync()
		{
			return await _productsRepostiory.GetAllProductAsync();
		}

        public async Task<List<Product>> GetProductsByBrandIdAsync(int brandId)
        {
            return await _productsRepostiory.GetProductByBrandIdAsync(brandId);
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
		{
			// Service này chỉ đơn giản gọi Repository, hoặc thêm logic xử lý nếu có
			return await _productsRepostiory.GetProductsByCategoryIdAsync(categoryId);
		}

        public async Task<Shopping_Store_Data.Model.Product?> GetProductByIdAsync(int id)
        {
            return await _productsRepostiory.GetProductByIdAsync(id);
        }

		public async Task<List<Product>> GetAllProducts()
		{
			return await _productsRepostiory.GetAllProducts();
		}

        public async Task DeleteProductAsync(int id)
        {
             await _productsRepostiory.DeleteAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productsRepostiory.AddProductAsync(product);
        }

        public async Task<bool> IsProductSlugExistsAsync(string slug) // <--- Triển khai phương thức MỚI
        {
            return await _productsRepostiory.IsSlugExistsAsync(slug);
        }

        public async Task UpdateProductAsync(Product product) // <--- Triển khai phương thức MỚI
        {
            await _productsRepostiory.UpdateProductAsync(product);
        }

        public async Task<bool> IsProductSlugExistsForOtherProductAsync(string slug, int productId) // <--- Triển khai phương thức MỚI
        {
            return await _productsRepostiory.IsSlugExistsForOtherProductAsync(slug, productId);
        }
    }
}
