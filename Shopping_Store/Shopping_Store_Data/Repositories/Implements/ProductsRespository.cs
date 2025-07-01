using Microsoft.EntityFrameworkCore;
using Shopping_Store_Data.Model;
using Shopping_Store_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Repositories.Implements
{
	public class ProductsRespository : IProductsRespository
	{
		private readonly DataContext _context;

		public ProductsRespository(DataContext context)
		{
			_context = context;
		}

		public async Task<List<Product>> GetAllProductAsync()
		{
			return await _context.Products.ToListAsync();
		}

        public async Task<List<Product>> GetProductByBrandIdAsync(int brandId)
        {
            return await _context.Products
                                 .Where(p => p.BrandId == brandId)
                                 .OrderByDescending(p => p.Id) // Giữ lại logic sắp xếp
                                 .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
		{
			return await _context.Products
								 .Where(p => p.CategoryId == categoryId)
								 .OrderByDescending(p => p.Id) // Giữ lại logic sắp xếp
								 .ToListAsync();
		}

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            // Sử dụng Include để tải Category và Brand cùng với Product
            // Điều này giúp tránh lỗi "Object reference not set to an instance of an object"
            // khi truy cập item.Category.Name hoặc item.Brand.Name trong View
            return await _context.Products
                                   .Include(p => p.Category)
                                   .Include(p => p.Brand)
                                   .FirstOrDefaultAsync(p => p.Id == id);
        }

		public async Task<List<Product>> GetAllProducts() // Đổi từ GetAllProductAsync nếu tên là GetAllProducts
		{
			return await _context.Products
								   .Include(p => p.Category) // <--- BAO GỒM THÔNG TIN DANH MỤC
								   .Include(p => p.Brand) // <--- BAO GỒM THÔNG TIN DANH MỤC
								   .OrderByDescending(p => p.Id) // <--- SẮP XẾP THEO ID GIẢM DẦN
								   .ToListAsync();
		}

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product!=null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetProductBySlugAsync(string slug) // <--- Triển khai phương thức MỚI
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<bool> IsSlugExistsAsync(string slug) // <--- Triển khai phương thức MỚI
        {
            return await _context.Products.AnyAsync(p => p.Slug == slug);
        }

        public async Task UpdateProductAsync(Product product) // <--- Triển khai phương thức MỚI
        {
           // _context.Products.Update(product); // Đánh dấu đối tượng là đã thay đổi
            await _context.SaveChangesAsync(); // Lưu thay đổi vào database
        }

        public async Task<bool> IsSlugExistsForOtherProductAsync(string slug, int productId) // <--- Triển khai phương thức MỚI
        {
            // Kiểm tra xem có sản phẩm nào khác ngoài sản phẩm hiện tại có cùng slug không
            return await _context.Products.AnyAsync(p => p.Slug == slug && p.Id != productId);
        }
    }
}
