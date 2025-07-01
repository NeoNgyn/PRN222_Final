using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Areas.Admin.Pages.Brands
{
    public class IndexModel : PageModel
    {
		private readonly IBrandsService _brandService; // Service cho Brands

		public IEnumerable<Brand>? Brands { get; set; } // Thu?c t�nh cho danh s�ch Brands

		public IndexModel(IBrandsService brandService)
		{
			_brandService = brandService;
		}

		public async Task OnGetAsync()
		{
			Brands = await _brandService.GetAllBrandAsync(); // L?y t?t c? Brands
		}
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var brandToDelete = await _brandService.GetBrandByIdAsync(id);
            if (brandToDelete != null)
            {
                await _brandService.DeleteBrandAsync(id);
                TempData["success"] = $"Brand '{brandToDelete.Name}' remove successfully.";
            }
            else
            {
                TempData["error"] = "Kh�ng t�m th?y th??ng hi?u ?? x�a.";
            }
            return RedirectToPage(); // Chuy?n h??ng v? trang danh s�ch Brands
        }
    }
}
