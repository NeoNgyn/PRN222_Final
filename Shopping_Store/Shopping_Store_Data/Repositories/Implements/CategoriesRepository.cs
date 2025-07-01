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
	public class CategoriesRepository : ICategoriesRepository
	{
		private readonly DataContext _context;

		public CategoriesRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<List<Category>> GetAllCategoriesAsync()
		{
			return await _context.Categories.ToListAsync();
		}

		public async Task<Category> GetCategoryBySlugAsync(string slug)
		{
			return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
		}

		public async Task AddCategoryAsync(Category category) // <-- Triển khai phương thức MỚI
		{
			await _context.Categories.AddAsync(category); // Thêm entity vào DbSet
			await _context.SaveChangesAsync(); // Lưu thay đổi vào database
		}

		public async Task<bool> IsSlugExistsAsync(string slug) // <-- TRIỂN KHAI HÀM NÀY
		{
			return await _context.Categories.AnyAsync(c => c.Slug == slug);
		}

		public async Task<bool> IsSlugExistsForOtherCategoryAsync(string slug, int categoryId) // (Nếu bạn đã thêm cho Edit)
		{
			return await _context.Categories.AnyAsync(c => c.Slug == slug && c.Id != categoryId);
		}

		public async Task<Category?> GetCategoryByIdAsync(int id) // <-- TRIỂN KHAI HÀM NÀY
		{
			return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task UpdateCategoryAsync(Category category) // <-- TRIỂN KHAI HÀM NÀY
		{
			// Entity Framework đã tự động theo dõi 'category' nếu nó được load từ DbContext trước đó.
			// Nếu 'category' đến từ form, bạn có thể cần Attach/Update nó trước.
			// Tuy nhiên, cách bạn sử dụng trong EditModel là lấy đối tượng từ DB ra rồi cập nhật,
			// nên chỉ cần SaveChangesAsync() là đủ.
			await _context.SaveChangesAsync(); // <-- Dòng này sẽ lưu những thay đổi
		}

		public async Task DeleteCategoryAsync(int id) // <-- Triển khai phương thức MỚI
		{
			var categoryToDelete = await _context.Categories.FindAsync(id); // Tìm danh mục theo ID
			if (categoryToDelete != null)
			{
				_context.Categories.Remove(categoryToDelete); // Đánh dấu đối tượng để xóa
				await _context.SaveChangesAsync(); // Lưu thay đổi vào database
			}
		}
	}
}
