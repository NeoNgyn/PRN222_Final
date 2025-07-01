using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Areas.Admin.Pages.Categories
{
    [Area("Admin")]
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICategoriesServies _categoryService;

        // Thu?c t�nh ?? ch?a danh s�ch Categories
        public IEnumerable<Category>? Categories { get; set; }

        public IndexModel(ICategoriesServies categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task OnGetAsync()
        {
            // L?y t?t c? danh m?c t? CategoriesService
            Categories = await _categoryService.GetAllCategoriesAsync(); // <-- H�m n�y s? fetch d? li?u
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var categoryToDelete = await _categoryService.GetCategoryByIdAsync(id); // L?y t�n danh m?c ?? th�ng b�o
            if (categoryToDelete != null)
            {
                await _categoryService.DeleteCategoryAsync(id); // G?i service ?? x�a danh m?c
                TempData["success"] = $"Category '{categoryToDelete.Name}' remove successfully."; // Th�m th�ng b�o
            }
            else
            {
                TempData["error"] = "No found product to remove."; // Th�m th�ng b�o l?i
            }
            return RedirectToPage(); // Chuy?n h??ng v? ch�nh trang hi?n t?i ?? c?p nh?t danh s�ch
        }
    }
}
