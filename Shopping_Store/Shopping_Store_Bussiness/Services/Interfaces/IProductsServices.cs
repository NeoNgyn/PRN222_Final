using Shopping_Store_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Bussiness.Services.Interfaces
{
	public interface IProductsServices
	{
		Task<List<Product>> GetAllProductAsync();
		Task<List<Product>> GetAllProducts();
		Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
		Task<List<Product>> GetProductsByBrandIdAsync(int brandId);
        Task<Product?> GetProductByIdAsync(int id);
        Task DeleteProductAsync(int id);
		Task AddProductAsync(Product product);
        Task<bool> IsProductSlugExistsAsync(string slug);
        Task UpdateProductAsync(Product product); 
        Task<bool> IsProductSlugExistsForOtherProductAsync(string slug, int intProductId);
    }
}
