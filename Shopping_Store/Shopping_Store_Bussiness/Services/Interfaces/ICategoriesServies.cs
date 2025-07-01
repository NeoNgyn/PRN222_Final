using Shopping_Store_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Bussiness.Services.Interfaces
{
	public interface ICategoriesServies
	{
		Task<List<Category>> GetAllCategoriesAsync();
		Task<Category> GetCategoryBySlugAsync(string slug);
        Task<Category?> GetCategoryByIdAsync(int id); // <-- THÊM DÒNG NÀY
        Task UpdateCategoryAsync(Category category); // <-- THÊM DÒNG NÀY
        Task AddCategoryAsync(Category category);
		Task<bool> IsCategorySlugExistsAsync(string slug); // <-- THÊM DÒNG NÀY
		Task<bool> IsCategorySlugExistsForOtherCategoryAsync(string slug, int categoryId);
        Task DeleteCategoryAsync(int id);
    }
}
