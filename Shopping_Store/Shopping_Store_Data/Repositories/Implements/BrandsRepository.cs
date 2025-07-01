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
	public class BrandsRepository : IBrandsRepository
	{
		private readonly DataContext _context;

		public BrandsRepository(DataContext context)
		{
			_context = context;
		} 
		 
		public async Task<List<Brand>> GetAllBrandAsync()
		{
			return await _context.Brands.ToListAsync();
		}

		public async Task<Brand> GetBrandBySlugAsync(string slug)
		{
			return await _context.Brands.FirstOrDefaultAsync(c => c.Slug == slug);
		}

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            // Khi đối tượng 'brand' được truyền vào đây, nếu nó đã được load từ DbContext trước đó,
            // EF sẽ tự động theo dõi thay đổi và không cần _dbContext.Brands.Update(brand) nữa.
            // Nếu nó là một đối tượng 'detached' (chưa được theo dõi), bạn có thể dùng Update,
            // nhưng cách an toàn hơn khi update là load lại rồi cập nhật như trong Page Model.
            await _context.SaveChangesAsync(); // <-- Dòng này sẽ lưu những thay đổi
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brandToDelete = await _context.Brands.FindAsync(id); // Tìm Brand theo ID
            if (brandToDelete != null)
            {
                _context.Brands.Remove(brandToDelete); // Đánh dấu đối tượng để xóa
                await _context.SaveChangesAsync(); // Lưu thay đổi vào database
            }
        }

        public async Task<bool> IsSlugExistsAsync(string slug)
        {
            return await _context.Brands.AnyAsync(b => b.Slug == slug); // Kiểm tra xem có Brand nào có slug này không
        }

        public async Task<bool> IsSlugExistsForOtherBrandAsync(string slug, int brandId)
        {
            // Kiểm tra xem có Brand nào KHÁC (không phải Brand hiện tại) có cùng slug không
            return await _context.Brands.AnyAsync(b => b.Slug == slug && b.Id != brandId);
        }
    }
}
