using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Shopping_Store.Pages
{
    public class ErrorsModel : PageModel
    {
        // Thu?c t�nh ?? nh?n m� tr?ng th�i t? URL
        [BindProperty(SupportsGet = true)]
        public int StatusCode { get; set; }

        public string RequestId { get; set; } = string.Empty; // ?? hi?n th? ID c?a request l?i

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); // Ki?m tra xem c� RequestId ?? hi?n th? kh�ng

        // Handler cho HTTP GET
        public void OnGet()
        {
            // L?y RequestId n?u c� (h?u �ch cho vi?c debug)
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // B?n c� th? th�m logic ? ?�y ?? hi?n th? th�ng b�o c? th? d?a v�o StatusCode
            // V� d?: if (StatusCode == 404) { /* ... */ }
            // if (StatusCode == 403) { /* ... */ }
        }
    }
}
