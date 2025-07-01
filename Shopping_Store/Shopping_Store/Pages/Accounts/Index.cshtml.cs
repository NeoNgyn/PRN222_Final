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

		// Thu?c t�nh ?? h?ng ReturnUrl (t? query string ho?c header)
		[BindProperty(SupportsGet = true)]
		public string? ReturnUrl { get; set; } // S? kh?p v?i LoginViewModel.returnUrl n?u n� c� trong form

		public IndexModel(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task OnGetAsync(string? returnUrl = null) // Nh?n returnUrl t? GET request
		{
			// Reset m?t kh?u khi trang load ?? kh�ng gi? gi� tr? c? sau l?i validation
			LoginInput.Password = string.Empty;

			// L?y ReturnUrl n?u c� t? query string
			ReturnUrl = returnUrl ?? Url.Content("~/"); // N?u returnUrl l� null, ??t m?c ??nh l� trang ch?
		}

		public async Task<IActionResult> OnPostAsync() // Handler cho POST request
		{
			// 1. Validation ban ??u c?a LoginInput (UserName, Password)
			if (!ModelState.IsValid)
			{
				return Page(); // Hi?n th? l?i form v?i l?i validation ban ??u
			}

			// 2. T�m ng??i d�ng
			var user = await _userManager.FindByNameAsync(LoginInput.UserName);

			// 3. X? l� khi kh�ng t�m th?y ng??i d�ng
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "T�n ng??i d�ng kh�ng t?n t?i.");
				return Page(); // <-- Tr? v? Page() ?�NG
			}

			// 4. ??ng nh?p b?ng m?t kh?u
			// LoginInput.RememberMe kh�ng c� trong ViewModel c?a b?n, n�n t�i s? b? qua.
			var result = await _signInManager.PasswordSignInAsync(user, LoginInput.Password, isPersistent: false, lockoutOnFailure: false);

			// 5. X? l� k?t qu? ??ng nh?p
			if (result.Succeeded)
			{
				TempData["SuccessMessage"] = $"Ch�o m?ng {user.UserName}!";
				// CHUY?N H??NG TH�NH C�NG V? RETURN URL
				// ... (logic chuy?n h??ng v? ReturnUrl) ...
				return RedirectToPage("/Index"); // Chuy?n h??ng th�nh c�ng v? trang ch? m?c ??nh
			}
			else if (result.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "T�i kho?n c?a b?n ?� b? kh�a.");
				return Page(); // <-- Tr? v? Page() ?�NG
			}
			else if (result.IsNotAllowed)
			{
				ModelState.AddModelError(string.Empty, "??ng nh?p kh�ng th�nh c�ng. T�i kho?n c?a b?n kh�ng ???c ph�p ??ng nh?p.");
				return Page(); // <-- Tr? v? Page() ?�NG
			}
			else // Tr??ng h?p sai m?t kh?u
			{
				ModelState.AddModelError(string.Empty, "M?t kh?u kh�ng ?�ng."); // <-- Th�m l?i
				return Page(); // <-- TR? V? PAGE() ?�NG
			}
		}
	}
	}
	

