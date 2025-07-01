using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Areas.Admin.Pages.Categories
{
	[Area("Admin")]
	public class CreateCategoryModel(ICategoriesServies _categoryService) : PageModel
	{
		
		[BindProperty] // S? binding d? li?u t? form HTML vào ??i t??ng Category này
		public Category Category { get; set; } = new Category(); // Kh?i t?o ?? tránh null

		public IActionResult OnGet()
		{
			// Khi trang ???c t?i l?n ??u (GET request), ch? c?n tr? v? Page() ?? hi?n th? form tr?ng
			return Page();
		}

		public async Task<IActionResult> OnPostAsync() // Handler cho POST request khi form ???c submit
		{
			// B??c 1: T?o Slug t? tên danh m?c (n?u tên không r?ng)
			if (!string.IsNullOrEmpty(Category.Name))
			{
				Category.Slug = Category.Name.Replace(" ", "-").ToLower();
			}
			else
			{
				// N?u tên r?ng, Required attribute trên Name s? b?t l?i này,
				// nh?ng n?u không có Required ho?c b?n mu?n t? ki?m soát:
				// ModelState.AddModelError("Category.Name", "Tên danh m?c không ???c ?? tr?ng.");
			}

			// B??c 2: Ki?m tra Slug trùng l?p
			bool slugExists = await _categoryService.IsCategorySlugExistsAsync(Category.Slug);
			if (slugExists)
			{
				ModelState.AddModelError("Category.Name", "Tên danh m?c này ?ã t?n t?i (slug b? trùng). Vui lòng ch?n tên khác.");
			}

			// B??c 3: Ki?m tra t?ng quát Model Validation (bao g?m c? Data Annotations và l?i Slug)
			if (!ModelState.IsValid)
			{
				// N?u có l?i validation, tr? v? Page() ?? hi?n th? l?i form v?i các l?i
				return Page();
			}

			// B??c 4: Thêm danh m?c vào database (ch? khi form h?p l?)
			await _categoryService.AddCategoryAsync(Category);

			// B??c 5: Thêm thông báo thành công (s? d?ng TempData)
			TempData["success"] = $"Category '{Category.Name}'create successfully.";

			// B??c 6: Chuy?n h??ng v? trang danh sách danh m?c
			return RedirectToPage("Index"); // Redirect ??n Areas/Admin/Pages/Categories/Index.cshtml
		}
	}
}
