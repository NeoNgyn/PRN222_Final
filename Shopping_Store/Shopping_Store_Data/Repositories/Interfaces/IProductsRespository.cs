using Shopping_Store_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Repositories.Interfaces
{
	public interface IProductsRespository
	{
		Task<List<Product>> GetAllProductAsync();
		Task<List<Product>> GetAllProducts(); 
		Task DeleteAsync(int id);
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
		Task<List<Product>> GetProductByBrandIdAsync(int brandId);
        Task<Product?> GetProductByIdAsync(int id); 
        Task AddProductAsync(Product product);
        Task<Product?> GetProductBySlugAsync(string slug);
        Task<bool> IsSlugExistsAsync(string slug);
        Task UpdateProductAsync(Product product); 
        Task<bool> IsSlugExistsForOtherProductAsync(string slug, int productId);
    }
}
