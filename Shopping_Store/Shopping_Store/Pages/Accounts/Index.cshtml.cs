using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Data.Model;
using Shopping_Store_Data.Model.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Shopping_Store.Pages.Accounts
{
    public class IndexModel : PageModel
    {
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;

		// --- C?p nh?t [BindProperty] ?? s? d?ng LoginViewModel m?i c?a b?n ---
		[BindProperty]
		public LoginViewModel LoginInput { get; set; } = new LoginViewModel(); // S? d?ng LoginViewModel m?i

		// Thu?c tính ?? h?ng ReturnUrl (t? query string ho?c header)
		[BindProperty(SupportsGet = true)]
		public string? ReturnUrl { get; set; } // S? kh?p v?i LoginViewModel.returnUrl n?u nó có trong form

		public IndexModel(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task OnGetAsync(string? returnUrl = null) // Nh?n returnUrl t? GET request
		{
			// Reset m?t kh?u khi trang load ?? không gi? giá tr? c? sau l?i validation
			LoginInput.Password = string.Empty;

			// L?y ReturnUrl n?u có t? query string
			ReturnUrl = returnUrl ?? Url.Content("~/"); // N?u returnUrl là null, ??t m?c ??nh là trang ch?
		}

		public async Task<IActionResult> OnPostAsync() // Handler cho POST request
		{
			// 1. Validation ban ??u c?a LoginInput (UserName, Password)
			if (!ModelState.IsValid)
			{
				return Page(); // Hi?n th? l?i form v?i l?i validation ban ??u
			}

			// 2. Tìm ng??i dùng
			var user = await _userManager.FindByNameAsync(LoginInput.UserName);

			// 3. X? lý khi không tìm th?y ng??i dùng
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Tên ng??i dùng không t?n t?i.");
				return Page(); // <-- Tr? v? Page() ?ÚNG
			}

			// 4. ??ng nh?p b?ng m?t kh?u
			// LoginInput.RememberMe không có trong ViewModel c?a b?n, nên tôi s? b? qua.
			var result = await _signInManager.PasswordSignInAsync(user, LoginInput.Password, isPersistent: false, lockoutOnFailure: false);

			// 5. X? lý k?t qu? ??ng nh?p
			if (result.Succeeded)
			{
				TempData["SuccessMessage"] = $"Chào m?ng {user.UserName}!";
				// CHUY?N H??NG THÀNH CÔNG V? RETURN URL
				// ... (logic chuy?n h??ng v? ReturnUrl) ...
				return RedirectToPage("/Index"); // Chuy?n h??ng thành công v? trang ch? m?c ??nh
			}
			else if (result.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "Tài kho?n c?a b?n ?ã b? khóa.");
				return Page(); // <-- Tr? v? Page() ?ÚNG
			}
			else if (result.IsNotAllowed)
			{
				ModelState.AddModelError(string.Empty, "??ng nh?p không thành công. Tài kho?n c?a b?n không ???c phép ??ng nh?p.");
				return Page(); // <-- Tr? v? Page() ?ÚNG
			}
			else // Tr??ng h?p sai m?t kh?u
			{
				ModelState.AddModelError(string.Empty, "M?t kh?u không ?úng."); // <-- Thêm l?i
				return Page(); // <-- TR? V? PAGE() ?ÚNG
			}
		}
	}
	}
	

