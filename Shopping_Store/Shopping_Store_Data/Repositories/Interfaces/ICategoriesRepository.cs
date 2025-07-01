using Shopping_Store_Data.Model;
using Shopping_Store_Data.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Repositories.Interfaces
{
	public interface ICategoriesRepository
	{
		Task<List<Category>> GetAllCategoriesAsync();
		Task<Category> GetCategoryBySlugAsync(string slug);
		Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<Category?> GetCategoryByIdAsync(int id); // <-- THÊM DÒNG NÀY
        Task UpdateCategoryAsync(Category category); // <-- THÊM DÒNG NÀY
        Task<bool> IsSlugExistsAsync(string slug); // <-- THÊM DÒNG NÀY
		Task<bool> IsSlugExistsForOtherCategoryAsync(string slug, int categoryId);
	}
}
