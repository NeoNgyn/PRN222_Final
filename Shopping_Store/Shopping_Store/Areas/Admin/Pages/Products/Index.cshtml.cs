using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Areas.Admin.Pages.Products
{
    [Area("Admin")]
    public class IndexModel : PageModel
    {
        private readonly IProductsServices _productsService;
        public List<Product> Products { get; set; }
        public IndexModel(IProductsServices productsService)
        {
            _productsService = productsService;
        }
        public async Task OnGetAsync()
        {
            // L?y t?t c? s?n ph?m t? ProductsService
            Products = await _productsService.GetAllProducts();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var productToDelete = await _productsService.GetProductByIdAsync(id); // L?y t�n s?n ph?m ?? th�ng b�o
            if (productToDelete != null)
            {
                await _productsService.DeleteProductAsync(id);
                TempData["success"] = $"Product '{productToDelete.Name}' delete successfully"; // <--- TH�M TH�NG B�O TH�NH C�NG
            }
            else
            {
                TempData["error"] = "Kh�ng t�m th?y s?n ph?m ?? x�a."; // <--- TH�M TH�NG B�O L?I
            }
            return RedirectToPage();
        }
    }
}
