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
            var productToDelete = await _productsService.GetProductByIdAsync(id); // L?y tên s?n ph?m ?? thông báo
            if (productToDelete != null)
            {
                await _productsService.DeleteProductAsync(id);
                TempData["success"] = $"Product '{productToDelete.Name}' delete successfully"; // <--- THÊM THÔNG BÁO THÀNH CÔNG
            }
            else
            {
                TempData["error"] = "Không tìm th?y s?n ph?m ?? xóa."; // <--- THÊM THÔNG BÁO L?I
            }
            return RedirectToPage();
        }
    }
}
