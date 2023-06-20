using AssetManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public string Message { get; set; }
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
                .Include(b => b.Role)
                .Where(x => x.Username.Equals(account.username))
                .Where(x => x.Password.Equals(account.password))
                .Where(x => x.Status == true)
                .FirstOrDefault();
            if (user != null && (user.RoleId == 1 || user.Role.RoleName.Equals("Admin")))
            {
                error = 0;
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("userlogin", user.Id+"");
                HttpContext.Session.SetString("role", "admin");
                return RedirectToPage("Index");
            }
            else if (user != null && (user.RoleId == 2 || user.Role.RoleName.Equals("User")))
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
				return Page();
            }
        }
    }
}
