using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Implements;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Pages.Categories
{
    [Route("/category/{Slug?}")]
    public class IndexModel(IProductsServices productsService, ICategoriesServies categoryService) : PageModel
    {
		private readonly IProductsServices _productsService = productsService;
		private readonly ICategoriesServies _categoryService = categoryService; 

		[BindProperty(SupportsGet = true)]
		public string? Slug { get; set; } 

		public List<Shopping_Store_Data.Model.Product>? Products { get; set; } 

		public Category? CurrentCategory { get; set; } 

		public async Task<IActionResult> OnGetAsync()
		{
			if (string.IsNullOrEmpty(Slug))
			{
				return RedirectToPage("/Index"); 
			}

			CurrentCategory = await _categoryService.GetCategoryBySlugAsync(Slug);

			if (CurrentCategory == null)
			{
				return RedirectToPage("/Index"); 
			}

			Products = await _productsService.GetProductsByCategoryIdAsync(CurrentCategory.Id);

			return Page();
		}
	}
}
