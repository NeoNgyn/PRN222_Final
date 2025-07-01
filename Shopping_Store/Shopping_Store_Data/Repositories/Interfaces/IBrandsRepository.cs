using Shopping_Store_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Data.Repositories.Interfaces
{
	public interface IBrandsRepository
	{
		Task<List<Brand>> GetAllBrandAsync();
        Task<Brand> GetBrandBySlugAsync(string slug);
        Task<Brand?> GetBrandByIdAsync(int id);
        Task AddBrandAsync(Brand brand);
        Task UpdateBrandAsync(Brand brand);
        Task DeleteBrandAsync(int id);
        Task<bool> IsSlugExistsAsync(string slug); // Kiểm tra slug có tồn tại khi thêm
        Task<bool> IsSlugExistsForOtherBrandAsync(string slug, int brandId);
    }
}
