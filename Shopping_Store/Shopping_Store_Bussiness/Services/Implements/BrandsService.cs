using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;
using Shopping_Store_Data.Repositories.Implements;
using Shopping_Store_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Store_Bussiness.Services.Implements
{
	public class BrandsService : IBrandsService
	{
		private readonly IBrandsRepository _brandsRepository;

		// Constructor injection for the category repository
	
		public BrandsService(IBrandsRepository brandsRepository)
		{
			_brandsRepository = brandsRepository;
		}

		public async Task<List<Brand>> GetAllBrandAsync()
		{
			return await _brandsRepository.GetAllBrandAsync();
		}

        public async Task<Brand> GetBrandBySlugAsync(string slug)
        {
            return await _brandsRepository.GetBrandBySlugAsync(slug);
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _brandsRepository.GetBrandByIdAsync(id);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            await _brandsRepository.AddBrandAsync(brand);
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            await _brandsRepository.UpdateBrandAsync(brand);
        }

        public async Task DeleteBrandAsync(int id)
        {
            await _brandsRepository.DeleteBrandAsync(id);
        }

        public async Task<bool> IsBrandSlugExistsAsync(string slug)
        {
            return await _brandsRepository.IsSlugExistsAsync(slug);
        }

        public async Task<bool> IsBrandSlugExistsForOtherBrandAsync(string slug, int brandId)
        {
            return await _brandsRepository.IsSlugExistsForOtherBrandAsync(slug, brandId);
        }
    }
}
