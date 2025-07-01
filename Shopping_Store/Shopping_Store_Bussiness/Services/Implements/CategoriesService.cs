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
	public class CategoriesService : ICategoriesServies
	{
		private readonly ICategoriesRepository _categoryRepository; // Giả sử bạn có ICategoryRepository

		public CategoriesService(ICategoriesRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<List<Category>> GetAllCategoriesAsync()
		{
			return await _categoryRepository.GetAllCategoriesAsync();
		}

		public async Task<Category> GetCategoryBySlugAsync(string slug)
		{
			return await _categoryRepository.GetCategoryBySlugAsync(slug);
		}

		public async Task AddCategoryAsync(Category category) // <-- Triển khai phương thức MỚI
		{
			await _categoryRepository.AddCategoryAsync(category); // Gọi phương thức của Repository
		}

		public async Task<bool> IsCategorySlugExistsAsync(string slug) // <-- TRIỂN KHAI HÀM NÀY
		{
			return await _categoryRepository.IsSlugExistsAsync(slug); // Gọi hàm từ Repository
		}

		public async Task<bool> IsCategorySlugExistsForOtherCategoryAsync(string slug, int categoryId) // (Nếu bạn đã thêm cho Edit)
		{
			return await _categoryRepository.IsSlugExistsForOtherCategoryAsync(slug, categoryId);
		}

        public async Task<Category?> GetCategoryByIdAsync(int id) // <-- TRIỂN KHAI HÀM NÀY
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task UpdateCategoryAsync(Category category) // <-- TRIỂN KHAI HÀM NÀY
        {
            await _categoryRepository.UpdateCategoryAsync(category); // Gọi hàm từ Repository
        }
        public async Task DeleteCategoryAsync(int id) // <-- Triển khai phương thức MỚI
        {
            await _categoryRepository.DeleteCategoryAsync(id); // Gọi phương thức của Repository
        }
    }
}
