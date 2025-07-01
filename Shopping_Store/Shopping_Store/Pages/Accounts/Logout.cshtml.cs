using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping_Store_Data.Model;

namespace Shopping_Store.Pages.Accounts
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager; // SignInManager cho l?p User c?a b?n

        public LogoutModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync(); 

            TempData["SuccessMessage"] = "B?n ?ã ??ng xu?t thành công.";

            return RedirectToPage("/Index");
        }

    }
}

