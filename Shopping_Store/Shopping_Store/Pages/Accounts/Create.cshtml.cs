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
		private readonly SignInManager<User> _signInManager; // SignInManager ?? ??ng nh?p sau khi ??ng k�

		// --- Bind User Model tr?c ti?p (s? h?ng UserName, Email, Occupation) ---
		[BindProperty]
		public User AppUser { get; set; } = new User(); // ??i t�n bi?n ?? tr�nh nh?m l?n v?i t? kh�a 'User'

		// --- Thu?c t�nh ri�ng ?? h?ng M?T KH?U t? form (KH�NG n?m trong AppUser) ---
		[BindProperty]
		[Required(ErrorMessage = "M?t kh?u l� b?t bu?c.")]
		[StringLength(100, ErrorMessage = "{0} ph?i d�i �t nh?t {2} v� t?i ?a {1} k� t?.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		// --- Thu?c t�nh ri�ng ?? h?ng X�C NH?N M?T KH?U ---
		[BindProperty]
		[DataType(DataType.Password)]
		[Display(Name = "X�c nh?n m?t kh?u")]
		[Compare("Password", ErrorMessage = "M?t kh?u v� m?t kh?u x�c nh?n kh�ng kh?p.")]
		public string ConfirmPassword { get; set; } = string.Empty;


		public CreateModel(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public void OnGet()
		{
			// Reset c�c tr??ng m?t kh?u khi trang ???c load (?? kh�ng gi? gi� tr? c? sau l?i validation)
			Password = string.Empty;
			ConfirmPassword = string.Empty;
		}

		// Handler cho HTTP POST khi form ??ng k� ???c submit
		public async Task<IActionResult> OnPostAsync() // T�n handler m?c ??nh cho POST
		{
			// L?U � QUAN TR?NG:
			// Khi binding tr?c ti?p User model, validation cho UserName v� Email (t? IdentityUser)
			// c?ng s? ???c th?c hi?n t? ??ng.
			// C�c thu?c t�nh Password v� ConfirmPassword c?ng s? ???c validate.

			// B??c 1: Ki?m tra ModelState.IsValid T?NG TH?
			if (!ModelState.IsValid)
			{
				// N?u d? li?u form kh�ng h?p l?, hi?n th? l?i trang v?i l?i validation
				return Page();
			}

			// B??c 2: T?o t�i kho?n ng??i d�ng b?ng UserManager
			// userManager.CreateAsync s? l?y UserName v� Email t? AppUser, v� m?t kh?u t? bi?n Password
			var result = await _userManager.CreateAsync(AppUser, Password);

			if (result.Succeeded)
			{
				// ??ng nh?p ng??i d�ng ngay sau khi ??ng k� (t�y ch?n)
				await _signInManager.SignInAsync(AppUser, isPersistent: false);

				TempData["success"] = $"??ng k� th�nh c�ng! Ch�o m?ng, {AppUser.UserName}!";
				return RedirectToPage("/accounts/Index"); // Chuy?n h??ng ??n trang ch?
			}

			// X? l� c�c l?i n?u t?o t�i kho?n th?t b?i (v� d?: Username/Email ?� t?n t?i)
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description); // Th�m l?i Identity v�o ModelState
			}

			// N?u c� l?i, hi?n th? l?i form v?i l?i.
			return Page();
		}
	}
}
