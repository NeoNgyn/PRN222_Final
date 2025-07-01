using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Shopping_Store.Pages
{
    public class ErrorsModel : PageModel
    {
        // Thu?c tính ?? nh?n mã tr?ng thái t? URL
        [BindProperty(SupportsGet = true)]
        public int StatusCode { get; set; }

        public string RequestId { get; set; } = string.Empty; // ?? hi?n th? ID c?a request l?i

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); // Ki?m tra xem có RequestId ?? hi?n th? không

        // Handler cho HTTP GET
        public void OnGet()
        {
            // L?y RequestId n?u có (h?u ích cho vi?c debug)
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // B?n có th? thêm logic ? ?ây ?? hi?n th? thông báo c? th? d?a vào StatusCode
            // Ví d?: if (StatusCode == 404) { /* ... */ }
            // if (StatusCode == 403) { /* ... */ }
        }
    }
}
