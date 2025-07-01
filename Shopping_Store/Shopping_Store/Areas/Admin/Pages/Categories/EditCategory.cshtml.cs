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
            if (Id == 0) // Ki?m tra n?u ID không h?p l? (ví d?: /Edit không có ID)
            {
                return NotFound(); // Ho?c chuy?n h??ng v? trang danh sách
            }

            Category = await categoryService.GetCategoryByIdAsync(Id); // L?y danh m?c t? database

            if (Category == null)
            {
                return NotFound(); // Danh m?c không t?n t?i
            }
            return Page(); // Hi?n th? form v?i d? li?u danh m?c
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // B??c 1: T?o Slug t? tên danh m?c (n?u tên không r?ng)
            if (!string.IsNullOrEmpty(Category.Name))
            {
                string cleanedSlug = Regex.Replace(Category.Name.ToLower(), @"[^a-z0-9\s-]", "");
                cleanedSlug = Regex.Replace(cleanedSlug, @"\s+", "-");
                cleanedSlug = cleanedSlug.Trim('-');
                Category.Slug = cleanedSlug;
            }
            else
            {
                Category.Slug = null; // N?u tên r?ng, slug c?ng s? r?ng/null
            }

            // B??c 2: Ki?m tra Slug có giá tr? sau khi t?o và làm s?ch
            if (string.IsNullOrEmpty(Category.Slug))
            {
                ModelState.AddModelError("Category.Name", "Tên danh m?c không th? t?o ra m?t Slug h?p l?.");
            }
            else // Ch? ki?m tra trùng l?p n?u Slug ?ã ???c t?o và không r?ng
            {
                // B??c 3: Ki?m tra Slug trùng l?p v?i các danh m?c KHÁC (s? d?ng Category.Id ?? lo?i tr? chính nó)
                bool slugExists = await categoryService.IsCategorySlugExistsForOtherCategoryAsync(Category.Slug, Category.Id);
                if (slugExists)
                {
                    ModelState.AddModelError("Category.Name", "Tên danh m?c này ?ã t?n t?i (slug b? trùng). Vui lòng ch?n tên khác.");
                }
            }

            // B??c 4: Ki?m tra t?ng quát Model Validation (bao g?m c? Data Annotations và l?i Slug)
            if (!ModelState.IsValid)
            {
                // Quan tr?ng: N?u có l?i validation, b?n c?n l?y l?i ??i t??ng t? DB
                // ?? ??m b?o các thu?c tính không ???c binding t? form (ví d?: các thu?c tính ?i?u h??ng)
                // v?n ??y ?? khi form hi?n th? l?i.
                var originalCategory = await categoryService.GetCategoryByIdAsync(Category.Id);
                if (originalCategory != null)
                {
                    // Gán l?i các thu?c tính không ???c binding t? form n?u c?n,
                    // nh?ng th??ng ??i t??ng Category ???c binding là ?? ?? hi?n th? l?i.
                    // N?u b?n có các dropdown, b?n s? c?n LoadDropdownsAsync() ? ?ây.
                }
                return Page(); // Hi?n th? l?i form v?i l?i
            }

            // B??c 5: L?y danh m?c t? DB ?? Entity Framework theo dõi (n?u nó ch?a ???c theo dõi)
            // ?ây là cách an toàn nh?t ?? c?p nh?t m?t ??i t??ng ?ã có
            var categoryFromDb = await categoryService.GetCategoryByIdAsync(Category.Id);
            if (categoryFromDb == null)
            {
                return NotFound(); // Danh m?c không t?n t?i (hi?m khi x?y ra ? OnPost n?u OnGet thành công)
            }

            // B??c 6: C?p nh?t các thu?c tính c?a ??i t??ng ???c theo dõi b?ng d? li?u t? form
            categoryFromDb.Name = Category.Name;
            categoryFromDb.Description = Category.Description;
            categoryFromDb.Slug = Category.Slug; // Slug ?ã ???c t?o/ki?m tra ? trên
            categoryFromDb.status = Category.status; // ??m b?o tên thu?c tính ?úng (Status ho?c status)

            // B??c 7: L?u thay ??i vào database
            await categoryService.UpdateCategoryAsync(categoryFromDb);

            // B??c 8: Thêm thông báo thành công (s? d?ng TempData)
            TempData["success"] = $"Category '{categoryFromDb.Name}' edit successfully.";

            // B??c 9: Chuy?n h??ng v? trang danh sách danh m?c
            return RedirectToPage("Index");
        }
    }
}
