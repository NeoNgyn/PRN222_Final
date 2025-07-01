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
            // B1: T?o v� l�m s?ch Slug
            if (!string.IsNullOrEmpty(Brand.Name))
            {
                string cleanedSlug = Regex.Replace(Brand.Name.ToLower(), @"[^a-z0-9\s-]", "");
                cleanedSlug = Regex.Replace(cleanedSlug, @"\s+", "-");
                cleanedSlug = cleanedSlug.Trim('-');
                Brand.Slug = cleanedSlug;
            }
            else
            {
                Brand.Slug = null; // G�n r� r�ng l� null n?u t�n r?ng/kh�ng h?p l?
            }

            // B2: Ki?m tra Slug r?ng/null
            if (string.IsNullOrEmpty(Brand.Slug))
            {
                ModelState.AddModelError("Brand.Name", "T�n th??ng hi?u kh�ng th? t?o ra m?t Slug h?p l?.");
            }
            else // B3: Ki?m tra Slug tr�ng l?p
            {
                bool slugExists = await brandService.IsBrandSlugExistsAsync(Brand.Slug);
                if (slugExists)
                {
                    ModelState.AddModelError("Brand.Name", "T�n th??ng hi?u n�y ?� t?n t?i (slug b? tr�ng). Vui l�ng ch?n t�n kh�c.");
                }
            }

            // B4: Ki?m tra t?ng qu�t Model Validation
            if (!ModelState.IsValid)
            {
                return Page(); // Hi?n th? l?i form v?i l?i
            }

            // B5: Th�m th??ng hi?u v�o database
            await brandService.AddBrandAsync(Brand);

            // B6: Th�ng b�o th�nh c�ng
            TempData["success"] = $"Brand'{Brand.Name}' create successfully.";

            // B7: Chuy?n h??ng v? trang danh s�ch Brands
            return RedirectToPage("Index");
        }
    }
}
