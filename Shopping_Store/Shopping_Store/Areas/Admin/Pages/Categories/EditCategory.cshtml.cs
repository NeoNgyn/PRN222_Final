using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;
using System.Text.RegularExpressions;

namespace Shopping_Store.Areas.Admin.Pages.Categories
{
    [Area("Admin")]
    [Authorize]
    public class EditCategoryModel(ICategoriesServies categoryService) : PageModel
    {
        [BindProperty]
        public Category Category { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0) // Ki?m tra n?u ID kh�ng h?p l? (v� d?: /Edit kh�ng c� ID)
            {
                return NotFound(); // Ho?c chuy?n h??ng v? trang danh s�ch
            }

            Category = await categoryService.GetCategoryByIdAsync(Id); // L?y danh m?c t? database

            if (Category == null)
            {
                return NotFound(); // Danh m?c kh�ng t?n t?i
            }
            return Page(); // Hi?n th? form v?i d? li?u danh m?c
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // B??c 1: T?o Slug t? t�n danh m?c (n?u t�n kh�ng r?ng)
            if (!string.IsNullOrEmpty(Category.Name))
            {
                string cleanedSlug = Regex.Replace(Category.Name.ToLower(), @"[^a-z0-9\s-]", "");
                cleanedSlug = Regex.Replace(cleanedSlug, @"\s+", "-");
                cleanedSlug = cleanedSlug.Trim('-');
                Category.Slug = cleanedSlug;
            }
            else
            {
                Category.Slug = null; // N?u t�n r?ng, slug c?ng s? r?ng/null
            }

            // B??c 2: Ki?m tra Slug c� gi� tr? sau khi t?o v� l�m s?ch
            if (string.IsNullOrEmpty(Category.Slug))
            {
                ModelState.AddModelError("Category.Name", "T�n danh m?c kh�ng th? t?o ra m?t Slug h?p l?.");
            }
            else // Ch? ki?m tra tr�ng l?p n?u Slug ?� ???c t?o v� kh�ng r?ng
            {
                // B??c 3: Ki?m tra Slug tr�ng l?p v?i c�c danh m?c KH�C (s? d?ng Category.Id ?? lo?i tr? ch�nh n�)
                bool slugExists = await categoryService.IsCategorySlugExistsForOtherCategoryAsync(Category.Slug, Category.Id);
                if (slugExists)
                {
                    ModelState.AddModelError("Category.Name", "T�n danh m?c n�y ?� t?n t?i (slug b? tr�ng). Vui l�ng ch?n t�n kh�c.");
                }
            }

            // B??c 4: Ki?m tra t?ng qu�t Model Validation (bao g?m c? Data Annotations v� l?i Slug)
            if (!ModelState.IsValid)
            {
                // Quan tr?ng: N?u c� l?i validation, b?n c?n l?y l?i ??i t??ng t? DB
                // ?? ??m b?o c�c thu?c t�nh kh�ng ???c binding t? form (v� d?: c�c thu?c t�nh ?i?u h??ng)
                // v?n ??y ?? khi form hi?n th? l?i.
                var originalCategory = await categoryService.GetCategoryByIdAsync(Category.Id);
                if (originalCategory != null)
                {
                    // G�n l?i c�c thu?c t�nh kh�ng ???c binding t? form n?u c?n,
                    // nh?ng th??ng ??i t??ng Category ???c binding l� ?? ?? hi?n th? l?i.
                    // N?u b?n c� c�c dropdown, b?n s? c?n LoadDropdownsAsync() ? ?�y.
                }
                return Page(); // Hi?n th? l?i form v?i l?i
            }

            // B??c 5: L?y danh m?c t? DB ?? Entity Framework theo d�i (n?u n� ch?a ???c theo d�i)
            // ?�y l� c�ch an to�n nh?t ?? c?p nh?t m?t ??i t??ng ?� c�
            var categoryFromDb = await categoryService.GetCategoryByIdAsync(Category.Id);
            if (categoryFromDb == null)
            {
                return NotFound(); // Danh m?c kh�ng t?n t?i (hi?m khi x?y ra ? OnPost n?u OnGet th�nh c�ng)
            }

            // B??c 6: C?p nh?t c�c thu?c t�nh c?a ??i t??ng ???c theo d�i b?ng d? li?u t? form
            categoryFromDb.Name = Category.Name;
            categoryFromDb.Description = Category.Description;
            categoryFromDb.Slug = Category.Slug; // Slug ?� ???c t?o/ki?m tra ? tr�n
            categoryFromDb.status = Category.status; // ??m b?o t�n thu?c t�nh ?�ng (Status ho?c status)

            // B??c 7: L?u thay ??i v�o database
            await categoryService.UpdateCategoryAsync(categoryFromDb);

            // B??c 8: Th�m th�ng b�o th�nh c�ng (s? d?ng TempData)
            TempData["success"] = $"Category '{categoryFromDb.Name}' edit successfully.";

            // B??c 9: Chuy?n h??ng v? trang danh s�ch danh m?c
            return RedirectToPage("Index");
        }
    }
}
