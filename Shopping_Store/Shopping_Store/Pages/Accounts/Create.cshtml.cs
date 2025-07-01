using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Data.Model;
using System.ComponentModel.DataAnnotations;

namespace Shopping_Store.Pages.Accounts
{
    public class CreateModel : PageModel
    {
		private readonly UserManager<User> _userManager;     // UserManager cho l?p User c?a b?n
		private readonly SignInManager<User> _signInManager; // SignInManager ?? ??ng nh?p sau khi ??ng ký

		// --- Bind User Model tr?c ti?p (s? h?ng UserName, Email, Occupation) ---
		[BindProperty]
		public User AppUser { get; set; } = new User(); // ??i tên bi?n ?? tránh nh?m l?n v?i t? khóa 'User'

		// --- Thu?c tính riêng ?? h?ng M?T KH?U t? form (KHÔNG n?m trong AppUser) ---
		[BindProperty]
		[Required(ErrorMessage = "M?t kh?u là b?t bu?c.")]
		[StringLength(100, ErrorMessage = "{0} ph?i dài ít nh?t {2} và t?i ?a {1} ký t?.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		// --- Thu?c tính riêng ?? h?ng XÁC NH?N M?T KH?U ---
		[BindProperty]
		[DataType(DataType.Password)]
		[Display(Name = "Xác nh?n m?t kh?u")]
		[Compare("Password", ErrorMessage = "M?t kh?u và m?t kh?u xác nh?n không kh?p.")]
		public string ConfirmPassword { get; set; } = string.Empty;


		public CreateModel(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public void OnGet()
		{
			// Reset các tr??ng m?t kh?u khi trang ???c load (?? không gi? giá tr? c? sau l?i validation)
			Password = string.Empty;
			ConfirmPassword = string.Empty;
		}

		// Handler cho HTTP POST khi form ??ng ký ???c submit
		public async Task<IActionResult> OnPostAsync() // Tên handler m?c ??nh cho POST
		{
			// L?U Ý QUAN TR?NG:
			// Khi binding tr?c ti?p User model, validation cho UserName và Email (t? IdentityUser)
			// c?ng s? ???c th?c hi?n t? ??ng.
			// Các thu?c tính Password và ConfirmPassword c?ng s? ???c validate.

			// B??c 1: Ki?m tra ModelState.IsValid T?NG TH?
			if (!ModelState.IsValid)
			{
				// N?u d? li?u form không h?p l?, hi?n th? l?i trang v?i l?i validation
				return Page();
			}

			// B??c 2: T?o tài kho?n ng??i dùng b?ng UserManager
			// userManager.CreateAsync s? l?y UserName và Email t? AppUser, và m?t kh?u t? bi?n Password
			var result = await _userManager.CreateAsync(AppUser, Password);

			if (result.Succeeded)
			{
				// ??ng nh?p ng??i dùng ngay sau khi ??ng ký (tùy ch?n)
				await _signInManager.SignInAsync(AppUser, isPersistent: false);

				TempData["success"] = $"??ng ký thành công! Chào m?ng, {AppUser.UserName}!";
				return RedirectToPage("/accounts/Index"); // Chuy?n h??ng ??n trang ch?
			}

			// X? lý các l?i n?u t?o tài kho?n th?t b?i (ví d?: Username/Email ?ã t?n t?i)
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description); // Thêm l?i Identity vào ModelState
			}

			// N?u có l?i, hi?n th? l?i form v?i l?i.
			return Page();
		}
	}
}
