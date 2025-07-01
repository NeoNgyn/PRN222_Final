using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;
using System.IO;
using System.Threading.Tasks;

namespace Shopping_Store.Areas.Admin.Pages.Products
{

    [Area("Admin")]
    [Authorize]
    public class CreateProductModel(IProductsServices productsService, ICategoriesServies categoryService, IBrandsService brandService, IWebHostEnvironment webHostEnvironment) : PageModel
    {
        [BindProperty]
        public Product Product { get; set; } = new();

        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }

        private async Task LoadDropdownsAsync()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            Categories = new SelectList(categories, "Id", "Name");

            var brands = await brandService.GetAllBrandAsync();
            Brands = new SelectList(brands, "Id", "Name");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDropdownsAsync();
            return Page();
        }

        // PHI�N B?N OnPostAsync ?� ???C C?U TR�C L?I, LOGIC V� S?CH S? H?N
        public async Task<IActionResult> OnPostAsync()
        {
            // === B??C 1: TH?C HI?N C�C VALIDATION RI�NG ===

            // 1.1: Validation cho ?nh (b?t bu?c ph?i c� ?nh)
            if (Product.ImgUpload == null)
            {
                ModelState.AddModelError("Product.ImgUpload", "Vui l�ng ch?n ?nh cho s?n ph?m.");
            }

            // 1.2: Validation cho slug (n?u t�n s?n ph?m kh�ng r?ng)
            if (!string.IsNullOrEmpty(Product.Name))
            {
                Product.Slug = Product.Name.Replace(" ", "-").ToLower();
                bool slugExists = await productsService.IsProductSlugExistsAsync(Product.Slug);
                if (slugExists)
                {
                    ModelState.AddModelError("Product.Name", "T�n s?n ph?m n�y ?� t?n t?i, vui l�ng ch?n t�n kh�c.");
                }
            }

            // === B??C 2: KI?M TRA T?NG QU�T MODELSTATE ===
            // Bao g?m c? l?i t? DataAnnotations (vd: [Required]) v� c�c l?i ?� th�m ? tr�n
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(); // T?i l?i dropdown ?? hi?n th? l?i form
                return Page();
            }

            // === B??C 3: X? L� LOGIC N?U FORM H?P L? (HAPPY PATH) ===

            // X? l� l?u file ?nh
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "media/products"); // Th?ng nh?t ???ng d?n
            Directory.CreateDirectory(uploadsFolder); // T?o th? m?c n?u ch?a c�
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Product.ImgUpload.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Product.ImgUpload.CopyToAsync(fileStream);
            }
            Product.ImageUrl = uniqueFileName; // L?u t�n file v�o DB

            // Th�m s?n ph?m v�o c? s? d? li?u
            await productsService.AddProductAsync(Product);
            TempData["success"] = $"Product '{Product.Name}' added successful";

            // Chuy?n h??ng v? trang danh s�ch s?n ph?m
            return RedirectToPage("Index");
        }
    }
}