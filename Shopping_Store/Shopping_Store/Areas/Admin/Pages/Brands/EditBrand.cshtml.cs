using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;
using System.Text.RegularExpressions;

namespace Shopping_Store.Areas.Admin.Pages.Brands
{
    [Area("Admin")]
    public class EditBrandModel(IBrandsService brandService) : PageModel
    {
        [BindProperty]
        public Brand Brand { get; set; } = default!; // Brand Model (không kh?i t?o)

        [BindProperty(SupportsGet = true)] // ?? nh?n ID t? URL
        public int Id { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0)
            {
                return NotFound();
            }

            Brand = await brandService.GetBrandByIdAsync(Id); // L?y Brand t? DB

            if (Brand == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // B1: T?o và làm s?ch Slug
            if (!string.IsNullOrEmpty(Brand.Name))
            {
                string cleanedSlug = Regex.Replace(Brand.Name.ToLower(), @"[^a-z0-9\s-]", "");
                cleanedSlug = Regex.Replace(cleanedSlug, @"\s+", "-");
                cleanedSlug = cleanedSlug.Trim('-');
                Brand.Slug = cleanedSlug;
            }
            else
            {
                Brand.Slug = null; // Gán null n?u tên r?ng/không h?p l?
            }

            // B2: Ki?m tra Slug r?ng/null
            if (string.IsNullOrEmpty(Brand.Slug))
            {
                ModelState.AddModelError("Brand.Name", "Tên th??ng hi?u không th? t?o ra m?t Slug h?p l?.");
            }
            else // B3: Ki?m tra Slug trùng l?p v?i Brand KHÁC
            {
                bool slugExists = await brandService.IsBrandSlugExistsForOtherBrandAsync(Brand.Slug, Brand.Id);
                if (slugExists)
                {
                    ModelState.AddModelError("Brand.Name", "Tên th??ng hi?u này ?ã t?n t?i (slug b? trùng). Vui lòng ch?n tên khác.");
                }
            }

            // B4: Ki?m tra t?ng quát Model Validation
            if (!ModelState.IsValid)
            {
                return Page(); // Hi?n th? l?i form v?i l?i
            }

            // B5: L?y Brand t? DB ?? EF theo dõi và c?p nh?t
            var brandFromDb = await brandService.GetBrandByIdAsync(Brand.Id);
            if (brandFromDb == null)
            {
                return NotFound();
            }

            // B6: C?p nh?t các thu?c tính c?a Brand t? form vào ??i t??ng ???c theo dõi
            brandFromDb.Name = Brand.Name;
            brandFromDb.Description = Brand.Description;
            brandFromDb.Slug = Brand.Slug;
            brandFromDb.Status = Brand.Status; // ??m b?o tên thu?c tính ?úng (Status ho?c status)

            // B7: L?u thay ??i vào database
            await brandService.UpdateBrandAsync(brandFromDb);

            // B8: Thông báo thành công
            TempData["success"] = $"Brand'{brandFromDb.Name}' edit successfully.";

            // B9: Chuy?n h??ng v? trang danh sách Brands
            return RedirectToPage("Index");
        }
    }
}
