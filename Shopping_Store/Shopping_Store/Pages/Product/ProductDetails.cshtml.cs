using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;

namespace Shopping_Store.Pages.Product
{
    public class ProductDetailsModel : PageModel
    {

		private readonly IProductsServices _productsService;

		// Thu?c tính ?? nh?n ID s?n ph?m t? URL (ho?c query string)
		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }

		// Thu?c tính ?? ch?a s?n ph?m chi ti?t
		public Shopping_Store_Data.Model.Product? Product { get; set; }
		public ProductDetailsModel(IProductsServices productsService)
		{
			_productsService = productsService;
		}

		// Handler method OnGetAsync s? ???c g?i khi trang ???c truy c?p b?ng GET
		public async Task<IActionResult> OnGetAsync()
		{
			// Ki?m tra n?u ID không h?p l? ho?c không ???c cung c?p
			if (Id == 0)
			{
				return RedirectToPage("/Index"); // Chuy?n h??ng v? trang ch? ho?c trang l?i
			}

			// G?i service ?? l?y s?n ph?m theo ID
			Product = await _productsService.GetProductByIdAsync(Id);

			// Ki?m tra n?u không tìm th?y s?n ph?m
			if (Product == null)
			{
				return NotFound(); // Tr? v? l?i 404 Not Found
								   // Ho?c return RedirectToPage("/Error"); // Chuy?n h??ng ??n trang l?i tùy ch?nh
			}

			// Tr? v? Page ?? hi?n th? View v?i d? li?u s?n ph?m
			return Page();
		}
	}
}
