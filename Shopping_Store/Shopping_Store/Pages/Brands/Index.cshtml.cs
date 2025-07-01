using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store.Pages.Product;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Pages.Brands
{
    [Route("/brands/{Slug?}")]
    public class IndexModel(IProductsServices productsService, IBrandsService brandService) : PageModel
    {
        //private readonly IProductsServices _productsService = productsService;
        //private readonly IBrandsService _brandService = brandService;
        [BindProperty(SupportsGet = true)]
        public string? Slug { get; set; } // Làm nullable n?u có th? null

        public List<Shopping_Store_Data.Model.Product>? Products { get; set; }

        public Shopping_Store_Data.Model.Brand? CurrentBrand { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Slug))
            {
                return RedirectToPage("/Index"); // Ho?c x? lý khác
            }

            CurrentBrand = await brandService.GetBrandBySlugAsync(Slug);

            if (CurrentBrand == null)
            {
                return RedirectToPage("/Index"); // Ho?c trang l?i
            }

            Products = await productsService.GetProductsByCategoryIdAsync(CurrentBrand.Id);

            return Page();
        }
    }
}
