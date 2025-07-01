using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;
using System.Text.RegularExpressions;

namespace Shopping_Store.Areas.Admin.Pages.Brands
{
    [Area("Admin")]
    [Authorize]
    public class CreateBrandModel(IBrandsService brandService) : PageModel
    {        
        [BindProperty]
        public Brand Brand { get; set; } = new Brand(); // Brand Model

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
                Brand.Slug = null; // Gán rõ ràng là null n?u tên r?ng/không h?p l?
            }

            // B2: Ki?m tra Slug r?ng/null
            if (string.IsNullOrEmpty(Brand.Slug))
            {
                ModelState.AddModelError("Brand.Name", "Tên th??ng hi?u không th? t?o ra m?t Slug h?p l?.");
            }
            else // B3: Ki?m tra Slug trùng l?p
            {
                bool slugExists = await brandService.IsBrandSlugExistsAsync(Brand.Slug);
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

            // B5: Thêm th??ng hi?u vào database
            await brandService.AddBrandAsync(Brand);

            // B6: Thông báo thành công
            TempData["success"] = $"Brand'{Brand.Name}' create successfully.";

            // B7: Chuy?n h??ng v? trang danh sách Brands
            return RedirectToPage("Index");
        }
    }
}
