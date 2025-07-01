using Shopping_Store_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Bussiness.Services.Interfaces
{
    public interface IBrandsService
    {
		Task<List<Brand>> GetAllBrandAsync();
        Task<Brand> GetBrandBySlugAsync(string slug);
        Task<Brand?> GetBrandByIdAsync(int id);
        Task AddBrandAsync(Brand brand);
        Task UpdateBrandAsync(Brand brand);
        Task DeleteBrandAsync(int id);

        // Validation / Utility
        Task<bool> IsBrandSlugExistsAsync(string slug); // Kiểm tra slug có tồn tại khi thêm
        Task<bool> IsBrandSlugExistsForOtherBrandAsync(string slug, int brandId);
    }
}
