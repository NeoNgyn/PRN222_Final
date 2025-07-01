using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Areas.Admin.Pages.Products
{
    [Area("Admin")]
    // Nh?n ID s?n ph?m t? URL (ví d?: /Admin/Products/Edit?id=5)
    public class EditModel(IProductsServices productsService,
                      ICategoriesServies categoryService,
                      IBrandsService brandService,
                      IWebHostEnvironment webHostEnvironment) : PageModel
    {
        [BindProperty] // Dùng cho POST
        public Product Product { get; set; } // KHÔNG kh?i t?o ? ?ây, s? load t? DB

        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }

        // Thêm thu?c tính này ?? nh?n ID t? URL cho GET
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
                return NotFound(); // Ho?c chuy?n h??ng v? trang danh sách
            }

            Product = await productsService.GetProductByIdAsync(Id); // L?y s?n ph?m t? database

            if (Product == null)
            {
                return NotFound(); // S?n ph?m không t?n t?i
            }

            await LoadDropdownsAsync(); // T?i d? li?u cho dropdowns
            return Page(); // Hi?n th? form v?i d? li?u s?n ph?m
        }

        // Handler cho HTTP POST: X? lý khi form ???c submit ?? c?p nh?t
        public async Task<IActionResult> OnPostAsync()
        {
            var productFromDb = await productsService.GetProductByIdAsync(Product.Id);
            if (productFromDb == null)
            {
                return NotFound(); // S?n ph?m không t?n t?i ?? c?p nh?t
            }
            string oldImageUrl = productFromDb.ImageUrl;

            Product.Slug = Product.Name.Replace(" ", "-").ToLower();

            bool slugExists = await productsService.IsProductSlugExistsForOtherProductAsync(Product.Slug, Product.Id);
            if (slugExists)
            {
                ModelState.AddModelError("Product.Name", "Tên s?n ph?m này ?ã t?n t?i (slug b? trùng). Vui lòng ch?n tên khác.");
            }

            if (Product.ImgUpload != null)
            {

                var fileExtensionAttribute = new FileExtensionAttribute("jpg,png,jpeg");
                if (!fileExtensionAttribute.IsValid(Product.ImgUpload))
                {
                    ModelState.AddModelError("Product.ImgUpload", fileExtensionAttribute.ErrorMessage);
                }
            }

            // B??c 2: Ki?m tra t?ng quát ModelState
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            if (Product.ImgUpload != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                Directory.CreateDirectory(uploadsFolder); // ??m b?o th? m?c t?n t?i

                // XÓA ?NH C? N?U T?N T?I VÀ KHÔNG PH?I ?NH M?C ??NH
                if (!string.IsNullOrEmpty(oldImageUrl) && oldImageUrl != "default-product.png")
                {
                    string oldFilePath = Path.Combine(uploadsFolder, oldImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                        // Optional: Console.WriteLine($"?ã xóa ?nh c?: {oldFilePath}");
                    }
                }

                // L?U ?NH M?I
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Product.ImgUpload.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Product.ImgUpload.CopyToAsync(fileStream);
                }
                Product.ImageUrl = uniqueFileName; // C?p nh?t ImageUrl v?i tên file m?i
            }
            else // Ng??i dùng KHÔNG ch?n ?nh m?i, gi? nguyên ?nh c?
            {
                Product.ImageUrl = oldImageUrl; // Gán l?i URL ?nh c? (t? ProductFromDb) vào Product
            }

            // B??C 6: C?p nh?t các thu?c tính khác c?a Product (t? form) vào productFromDb
            // Sau ?ó m?i c?p nh?t productFromDb vào database
            productFromDb.Name = Product.Name;
            productFromDb.Description = Product.Description;
            productFromDb.Price = Product.Price;
            productFromDb.CategoryId = Product.CategoryId;
            productFromDb.BrandId = Product.BrandId;
            productFromDb.Slug = Product.Slug;
            productFromDb.ImageUrl = Product.ImageUrl;

            // B??c 4: C?p nh?t s?n ph?m vào database
            await productsService.UpdateProductAsync(Product);
            TempData["success"] = $"Product '{Product.Name}' edit successfully.";

            // B??c 5: Chuy?n h??ng v? trang danh sách
            return RedirectToPage("Index");
        }
    }
}

