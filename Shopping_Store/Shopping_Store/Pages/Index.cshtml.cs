using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store.Extensions;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductsServices _productsService; // Khai b�o service

        // Property ?? ch?a danh s�ch s?n ph?m s? ???c truy?n cho View
        public List<Shopping_Store_Data.Model.Product> Products { get; set; } // S? d?ng List<Product> ?? d? d�ng ki?m tra null/Any()

        // Ti�m IProductsServices v�o constructor c?a Page Model
        public IndexModel(IProductsServices productsService)
        {
            _productsService = productsService;
        }

        // Ph??ng th?c OnGetAsync s? ???c g?i khi trang ???c t?i b?ng HTTP GET
        public async Task OnGetAsync() // S? d?ng async Task OnGetAsync() v� service c?a b?n l� async
        {
            // G?i service ?? l?y t?t c? s?n ph?m b?t ??ng b?
            Products = (await _productsService.GetAllProductAsync()).ToList(); // L?y List t? IEnumerable<Product>
        }
		public async Task<IActionResult> OnPostAddToCart(int id, int quantity = 1) // Nh?n ProductId v� Quantity
		{
			// 1. L?y th�ng tin s?n ph?m t? database (d�ng ProductService)
			var productToAdd = await _productsService.GetProductByIdAsync(id);
			if (productToAdd == null)
			{
				// Tr? v? Not Found ho?c hi?n th? th�ng b�o l?i
				return NotFound();
			}

			// 2. L?y gi? h�ng hi?n t?i t? Session
			// Gi? s? gi? h�ng ???c l?u v?i key "Cart"
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

			// 3. C?p nh?t ho?c th�m s?n ph?m v�o gi? h�ng
			var existingItem = cart.FirstOrDefault(item => item.ProductId == id);
			if (existingItem != null)
			{
				// N?u s?n ph?m ?� c�, t?ng s? l??ng
				existingItem.Quantity += quantity;
			}
			else
			{
				// N?u ch?a c�, th�m s?n ph?m m?i v�o gi? h�ng
				cart.Add(new CartItem
				{
					ProductId = productToAdd.Id,
					ProductName = productToAdd.Name,
					Price = productToAdd.Price,
					Quantity = quantity,
					ImageUrl = productToAdd.ImageUrl // ??m b?o c� ImageUrl ?? hi?n th? trong gi? h�ng
				});
			}

			// 4. L?u gi? h�ng ?� c?p nh?t v�o Session
			HttpContext.Session.SetObjectAsJson("Cart", cart);

			// 5. Chuy?n h??ng sau khi th�m v�o gi? h�ng
			// C� th? chuy?n h??ng v? trang gi? h�ng:
			return RedirectToPage("/Cart/Index");
			// Ho?c chuy?n h??ng v? trang hi?n t?i ?? ng??i d�ng ti?p t?c mua s?m (nh?ng c?n th�ng b�o ?� th�m)
			// return RedirectToPage();
		}
	}
}
