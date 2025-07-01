using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store.Extensions;
using Shopping_Store_Bussiness.Services.Implements;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Model;
using Shopping_Store_Data.Model.ViewModels;

namespace Shopping_Store.Pages.Cart
{
    public class IndexModel : PageModel
    {
		private readonly IProductsServices _productsService;
		
		public CartItemViewModel CartVM { get; set; } = new CartItemViewModel(); 
        public IndexModel(IProductsServices productsService)
        {
            _productsService = productsService;
        }
        public IActionResult OnPostIncreaseQuantity(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var itemToUpdate = cart.FirstOrDefault(item => item.ProductId == id);

            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity++;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
			TempData["success"] = "Increase Items Successful";
			return RedirectToPage();
        }

        public IActionResult OnPostDecreaseQuantity(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var itemToUpdate = cart.FirstOrDefault(item => item.ProductId == id);

            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity--; // Gi?m s? l??ng ?i 1

                if (itemToUpdate.Quantity <= 0) // N?u s? l??ng <= 0, xóa s?n ph?m kh?i gi?
                {
                    cart.Remove(itemToUpdate);
                }
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart"); // Xóa h?n key "Cart" kh?i session
            }
            else
            {
                HttpContext.Session.SetObjectAsJson("Cart", cart); // L?u gi? hàng ?ã c?p nh?t
            }
			TempData["success"] = "Decrease Quantity Successful";
			return RedirectToPage(); // T?i l?i trang ?? c?p nh?t UI

        }
        public IActionResult OnPostUpdateQuantity(int id, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var itemToUpdate = cart.FirstOrDefault(item => item.ProductId == id);

            if (itemToUpdate != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(itemToUpdate);
                }
                else
                {
                    itemToUpdate.Quantity = quantity;
                }

                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                }
            }

            return RedirectToPage();
        }

        public void OnGet()
        {
            List<CartItem> cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var debug = TempData["Notification"]; 
            Console.WriteLine("?? TempData Notification: " + debug);


            CartVM.CartItems = cartItems;
            CartVM.TotalPrice = cartItems.Sum(item => item.Quantity * item.Price);
        }

        public async Task<IActionResult> OnPostAddToCart(int productId, int quantity = 1)
		{
			var productToAdd = await _productsService.GetProductByIdAsync(productId);
			if (productToAdd == null)
			{

				ModelState.AddModelError(string.Empty, "S?n ph?m không t?n t?i.");
				return NotFound();
			}

			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();


			var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

			if (existingItem != null)
			{
				existingItem.Quantity += quantity;
			}
			else
			{
				// N?u s?n ph?m ch?a có trong gi?, thêm m?i
				cart.Add(new CartItem
				{
					ProductId = productToAdd.Id,
					ProductName = productToAdd.Name,
					Price = productToAdd.Price,
					Quantity = quantity,
					ImageUrl = productToAdd.ImageUrl,
				});
			}

			HttpContext.Session.SetObjectAsJson("Cart", cart);

            TempData["success"] = "Add To Cart Successful";

            if (Request.Headers.ContainsKey("Referer"))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                // Fallback n?u không có Referer
                return RedirectToPage("/Index");
            }
            // Ho?c n?u b?n mu?n ? l?i trang hi?n t?i (ví d?: trang danh sách s?n ph?m)
            // thì return RedirectToPage(); và b?n có th? c?n thêm m?t thông báo popup
        }
        public IActionResult OnPostRemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == id);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);

                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                    
                }
                TempData["Notification"] = "??? S?n ph?m ?ã ???c xoá kh?i gi? hàng.";

            }
			TempData["success"] = "Remove Items Successful";

			return RedirectToPage(); // Trang s? reload, g?i l?i OnGet()
        }
        public async Task<IActionResult> OnPostClear() // ??i tên thành OnPostClear
        {
            HttpContext.Session.Remove("Cart");

            TempData["success"] = "?ã xóa t?t c? s?n ph?m kh?i gi? hàng thành công!"; 

            return RedirectToPage("./Index"); 
        }
    }
}
