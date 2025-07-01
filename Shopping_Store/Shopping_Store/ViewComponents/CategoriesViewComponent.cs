using Microsoft.AspNetCore.Mvc;
using Shopping_Store_Bussiness.Services.Implements;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data.Repositories.Interfaces;

namespace Shopping_Store.ViewComponents
{
	public class CategoriesViewComponent : ViewComponent
	{
		//private readonly ICategoriesInterface _categoryRepository;

		//// Constructor để inject ICategoryRepository
		//public CategoriesViewComponent(ICategoriesInterface categoryRepository)
		//{
		//	_categoryRepository = categoryRepository;
		//}

		//// Phương thức chính được gọi khi View Component được render
		//public async Task<IViewComponentResult> InvokeAsync()
		//{
		//	// Lấy danh sách danh mục từ repository
		//	var categories = await _categoryRepository.GetAllCategoriesAsync();

		//	// Trả về View tương ứng với danh sách categories
		//	return View(categories);
		//}
		private readonly ICategoriesServies _categoryService;
		public CategoriesViewComponent(ICategoriesServies categoryService)
		{
            _categoryService = categoryService;
		}

		// Phương thức InvokeAsync sẽ được gọi khi View Component được render
		public async Task<IViewComponentResult> InvokeAsync() // Lưu ý: Task<IViewComponentResult>
		{
			// *** Đảm bảo bạn dùng 'await' ở đây! ***
			var categories = await _categoryService.GetAllCategoriesAsync(); // Ví dụ: gọi phương thức async trong Service

			return View(categories); // Trả về View với dữ liệu Categories
		}
	}
}
