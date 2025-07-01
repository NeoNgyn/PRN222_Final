using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping_Store.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        [Area("Admin")]
        [Authorize]
        public void OnGet()
        {
        }
    }
}
