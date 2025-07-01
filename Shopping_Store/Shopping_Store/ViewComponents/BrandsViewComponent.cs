using Microsoft.AspNetCore.Mvc;
using Shopping_Store_Bussiness.Services.Interfaces;

namespace Shopping_Store.ViewComponents
{
    public class BrandsViewComponent : ViewComponent
	{
		private readonly IBrandsService _brandService;
		public BrandsViewComponent(IBrandsService brandService)
		{
			_brandService = brandService;
		}
		public async Task<IViewComponentResult> InvokeAsync() // Lưu ý: Task<IViewComponentResult>
		{
			// *** Đảm bảo bạn dùng 'await' ở đây! ***
			var categories = await _brandService.GetAllBrandAsync(); // Ví dụ: gọi phương thức async trong Service

			return View(categories); // Trả về View với dữ liệu Categories
		}

	}
}
