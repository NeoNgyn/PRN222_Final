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
        public Brand Brand { get; set; } = default!; // Brand Model (kh�ng kh?i t?o)

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
                Brand.Slug = null; // G�n null n?u t�n r?ng/kh�ng h?p l?
            }

            // B2: Ki?m tra Slug r?ng/null
            if (string.IsNullOrEmpty(Brand.Slug))
            {
                ModelState.AddModelError("Brand.Name", "T�n th??ng hi?u kh�ng th? t?o ra m?t Slug h?p l?.");
            }
            else // B3: Ki?m tra Slug tr�ng l?p v?i Brand KH�C
            {
                bool slugExists = await brandService.IsBrandSlugExistsForOtherBrandAsync(Brand.Slug, Brand.Id);
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

            // B5: L?y Brand t? DB ?? EF theo d�i v� c?p nh?t
            var brandFromDb = await brandService.GetBrandByIdAsync(Brand.Id);
            if (brandFromDb == null)
            {
                return NotFound();
            }

            // B6: C?p nh?t c�c thu?c t�nh c?a Brand t? form v�o ??i t??ng ???c theo d�i
            brandFromDb.Name = Brand.Name;
            brandFromDb.Description = Brand.Description;
            brandFromDb.Slug = Brand.Slug;
            brandFromDb.Status = Brand.Status; // ??m b?o t�n thu?c t�nh ?�ng (Status ho?c status)

            // B7: L?u thay ??i v�o database
            await brandService.UpdateBrandAsync(brandFromDb);

            // B8: Th�ng b�o th�nh c�ng
            TempData["success"] = $"Brand'{brandFromDb.Name}' edit successfully.";

            // B9: Chuy?n h??ng v? trang danh s�ch Brands
            return RedirectToPage("Index");
        }
    }
}
