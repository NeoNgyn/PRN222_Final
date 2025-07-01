using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Areas.Admin.Pages.Categories
{
	[Area("Admin")]
	public class CreateCategoryModel(ICategoriesServies _categoryService) : PageModel
	{
		
		[BindProperty] // S? binding d? li?u t? form HTML v�o ??i t??ng Category n�y
		public Category Category { get; set; } = new Category(); // Kh?i t?o ?? tr�nh null

		public IActionResult OnGet()
		{
			// Khi trang ???c t?i l?n ??u (GET request), ch? c?n tr? v? Page() ?? hi?n th? form tr?ng
			return Page();
		}

		public async Task<IActionResult> OnPostAsync() // Handler cho POST request khi form ???c submit
		{
			// B??c 1: T?o Slug t? t�n danh m?c (n?u t�n kh�ng r?ng)
			if (!string.IsNullOrEmpty(Category.Name))
			{
				Category.Slug = Category.Name.Replace(" ", "-").ToLower();
			}
			else
			{
				// N?u t�n r?ng, Required attribute tr�n Name s? b?t l?i n�y,
				// nh?ng n?u kh�ng c� Required ho?c b?n mu?n t? ki?m so�t:
				// ModelState.AddModelError("Category.Name", "T�n danh m?c kh�ng ???c ?? tr?ng.");
			}

			// B??c 2: Ki?m tra Slug tr�ng l?p
			bool slugExists = await _categoryService.IsCategorySlugExistsAsync(Category.Slug);
			if (slugExists)
			{
				ModelState.AddModelError("Category.Name", "T�n danh m?c n�y ?� t?n t?i (slug b? tr�ng). Vui l�ng ch?n t�n kh�c.");
			}

			// B??c 3: Ki?m tra t?ng qu�t Model Validation (bao g?m c? Data Annotations v� l?i Slug)
			if (!ModelState.IsValid)
			{
				// N?u c� l?i validation, tr? v? Page() ?? hi?n th? l?i form v?i c�c l?i
				return Page();
			}

			// B??c 4: Th�m danh m?c v�o database (ch? khi form h?p l?)
			await _categoryService.AddCategoryAsync(Category);

			// B??c 5: Th�m th�ng b�o th�nh c�ng (s? d?ng TempData)
			TempData["success"] = $"Category '{Category.Name}'create successfully.";

			// B??c 6: Chuy?n h??ng v? trang danh s�ch danh m?c
			return RedirectToPage("Index"); // Redirect ??n Areas/Admin/Pages/Categories/Index.cshtml
		}
	}
}
