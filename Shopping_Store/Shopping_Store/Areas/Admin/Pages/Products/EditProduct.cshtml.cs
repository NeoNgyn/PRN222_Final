using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Areas.Admin.Pages.Products
{
    [Area("Admin")]
    // Nh?n ID s?n ph?m t? URL (v� d?: /Admin/Products/Edit?id=5)
    public class EditModel(IProductsServices productsService,
                      ICategoriesServies categoryService,
                      IBrandsService brandService,
                      IWebHostEnvironment webHostEnvironment) : PageModel
    {
        [BindProperty] // D�ng cho POST
        public Product Product { get; set; } // KH�NG kh?i t?o ? ?�y, s? load t? DB

        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }

        // Th�m thu?c t�nh n�y ?? nh?n ID t? URL cho GET
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        private async Task LoadDropdownsAsync()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            Categories = new SelectList(categories, "Id", "Name");

            var brands = await brandService.GetAllBrandAsync();
            Brands = new SelectList(brands, "Id", "Name");
        }

        // Handler cho HTTP GET: T?i d? li?u s?n ph?m ?? hi?n th? form
        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == 0)
            {
                return NotFound(); // Ho?c chuy?n h??ng v? trang danh s�ch
            }

            Product = await productsService.GetProductByIdAsync(Id); // L?y s?n ph?m t? database

            if (Product == null)
            {
                return NotFound(); // S?n ph?m kh�ng t?n t?i
            }

            await LoadDropdownsAsync(); // T?i d? li?u cho dropdowns
            return Page(); // Hi?n th? form v?i d? li?u s?n ph?m
        }

        // Handler cho HTTP POST: X? l� khi form ???c submit ?? c?p nh?t
        public async Task<IActionResult> OnPostAsync()
        {
            var productFromDb = await productsService.GetProductByIdAsync(Product.Id);
            if (productFromDb == null)
            {
                return NotFound(); // S?n ph?m kh�ng t?n t?i ?? c?p nh?t
            }
            string oldImageUrl = productFromDb.ImageUrl;

            Product.Slug = Product.Name.Replace(" ", "-").ToLower();

            bool slugExists = await productsService.IsProductSlugExistsForOtherProductAsync(Product.Slug, Product.Id);
            if (slugExists)
            {
                ModelState.AddModelError("Product.Name", "T�n s?n ph?m n�y ?� t?n t?i (slug b? tr�ng). Vui l�ng ch?n t�n kh�c.");
            }

            if (Product.ImgUpload != null)
            {

                var fileExtensionAttribute = new FileExtensionAttribute("jpg,png,jpeg");
                if (!fileExtensionAttribute.IsValid(Product.ImgUpload))
                {
                    ModelState.AddModelError("Product.ImgUpload", fileExtensionAttribute.ErrorMessage);
                }
            }

            // B??c 2: Ki?m tra t?ng qu�t ModelState
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            if (Product.ImgUpload != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                Directory.CreateDirectory(uploadsFolder); // ??m b?o th? m?c t?n t?i

                // X�A ?NH C? N?U T?N T?I V� KH�NG PH?I ?NH M?C ??NH
                if (!string.IsNullOrEmpty(oldImageUrl) && oldImageUrl != "default-product.png")
                {
                    string oldFilePath = Path.Combine(uploadsFolder, oldImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                        // Optional: Console.WriteLine($"?� x�a ?nh c?: {oldFilePath}");
                    }
                }

                // L?U ?NH M?I
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Product.ImgUpload.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Product.ImgUpload.CopyToAsync(fileStream);
                }
                Product.ImageUrl = uniqueFileName; // C?p nh?t ImageUrl v?i t�n file m?i
            }
            else // Ng??i d�ng KH�NG ch?n ?nh m?i, gi? nguy�n ?nh c?
            {
                Product.ImageUrl = oldImageUrl; // G�n l?i URL ?nh c? (t? ProductFromDb) v�o Product
            }

            // B??C 6: C?p nh?t c�c thu?c t�nh kh�c c?a Product (t? form) v�o productFromDb
            // Sau ?� m?i c?p nh?t productFromDb v�o database
            productFromDb.Name = Product.Name;
            productFromDb.Description = Product.Description;
            productFromDb.Price = Product.Price;
            productFromDb.CategoryId = Product.CategoryId;
            productFromDb.BrandId = Product.BrandId;
            productFromDb.Slug = Product.Slug;
            productFromDb.ImageUrl = Product.ImageUrl;

            // B??c 4: C?p nh?t s?n ph?m v�o database
            await productsService.UpdateProductAsync(Product);
            TempData["success"] = $"Product '{Product.Name}' edit successfully.";

            // B??c 5: Chuy?n h??ng v? trang danh s�ch
            return RedirectToPage("Index");
        }
    }
}

