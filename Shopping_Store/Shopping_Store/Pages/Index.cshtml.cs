using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store.Extensions;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductsServices _productsService; // Khai báo service

        // Property ?? ch?a danh sách s?n ph?m s? ???c truy?n cho View
        public List<Shopping_Store_Data.Model.Product> Products { get; set; } // S? d?ng List<Product> ?? d? dàng ki?m tra null/Any()

        // Tiêm IProductsServices vào constructor c?a Page Model
        public IndexModel(IProductsServices productsService)
        {
            _productsService = productsService;
        }

        // Ph??ng th?c OnGetAsync s? ???c g?i khi trang ???c t?i b?ng HTTP GET
        public async Task OnGetAsync() // S? d?ng async Task OnGetAsync() vì service c?a b?n là async
        {
            // G?i service ?? l?y t?t c? s?n ph?m b?t ??ng b?
            Products = (await _productsService.GetAllProductAsync()).ToList(); // L?y List t? IEnumerable<Product>
        }
		public async Task<IActionResult> OnPostAddToCart(int id, int quantity = 1) // Nh?n ProductId và Quantity
		{
			// 1. L?y thông tin s?n ph?m t? database (dùng ProductService)
			var productToAdd = await _productsService.GetProductByIdAsync(id);
			if (productToAdd == null)
			{
				// Tr? v? Not Found ho?c hi?n th? thông báo l?i
				return NotFound();
			}

			// 2. L?y gi? hàng hi?n t?i t? Session
			// Gi? s? gi? hàng ???c l?u v?i key "Cart"
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

			// 3. C?p nh?t ho?c thêm s?n ph?m vào gi? hàng
			var existingItem = cart.FirstOrDefault(item => item.ProductId == id);
			if (existingItem != null)
			{
				// N?u s?n ph?m ?ã có, t?ng s? l??ng
				existingItem.Quantity += quantity;
			}
			else
			{
				// N?u ch?a có, thêm s?n ph?m m?i vào gi? hàng
				cart.Add(new CartItem
				{
					ProductId = productToAdd.Id,
					ProductName = productToAdd.Name,
					Price = productToAdd.Price,
					Quantity = quantity,
					ImageUrl = productToAdd.ImageUrl // ??m b?o có ImageUrl ?? hi?n th? trong gi? hàng
				});
			}

			// 4. L?u gi? hàng ?ã c?p nh?t vào Session
			HttpContext.Session.SetObjectAsJson("Cart", cart);

			// 5. Chuy?n h??ng sau khi thêm vào gi? hàng
			// Có th? chuy?n h??ng v? trang gi? hàng:
			return RedirectToPage("/Cart/Index");
			// Ho?c chuy?n h??ng v? trang hi?n t?i ?? ng??i dùng ti?p t?c mua s?m (nh?ng c?n thông báo ?ã thêm)
			// return RedirectToPage();
		}
	}
}
