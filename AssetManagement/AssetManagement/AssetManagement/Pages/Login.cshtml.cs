using AssetManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace AssetManagement.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
        public LoginModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }
        [BindProperty]
        public IList<User> users { get; set; } = default!;
        [BindProperty]
        public Account account { get; set; }
        [BindProperty]
        public int error { get; set; }
        public async Task OnGetAsync()
        {
            users = _context.Users.ToList();
        }

        public IActionResult OnPost()
        {
            User user = _context.Users
                .Where(x => x.Username.Equals(account.username))
                .Where(x => x.Password.Equals(account.password))
                .FirstOrDefault();
            if (user != null && user.RoleId == 1)
            {
                error = 0;
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("userlogin", user.Id+"");
                HttpContext.Session.SetString("role", "admin");
                return RedirectToPage("Index");
            }
            else if (user != null && user.RoleId == 2)
            {
                error = 0;
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("userlogin", user.Id+"");
                HttpContext.Session.SetString("role", "user");
                return RedirectToPage("Index");
            }
            else
            {
                error = 1;
                return RedirectToPage("Login");
            }
        }
    }
}
