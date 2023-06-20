using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetManagement.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("userLogin");

            return RedirectToPage("Index");
        }
    }
}
