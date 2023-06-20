using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;
using StockManagement.Models;

namespace StockManagement.Pages
{
    public class LoginModel : PageModel
    {
        private readonly StockManagementContext _context;
        
        public LoginModel(StockManagementContext context)
        {
            _context = context;
        }
        
        [BindProperty] public List<User> Users { get; set; } = default!;
        [BindProperty] public User Account { get; set; } = default!;
        [BindProperty] public int Error { get; set; }
        
        public void OnGetAsync()
        {
            Users = _context.Users.ToList();
        }
        
        public IActionResult OnPost()
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Username != null && x.Password != null && x.Username.Equals(Account.Username) &&
                    x.Password.Equals(Account.Password));
            switch (user)
            {
                case { RoleId: 1 }:
                {
                    Error = 0;
                    if (user.Username != null) HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetString("userLogin", user.Id.ToString());
                    HttpContext.Session.SetString("role", "admin");
                    return RedirectToPage("Assets/AssetBorrow");
                }
                case { RoleId: 2 }:
                {
                    Error = 0;
                    if (user.Username != null) HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetString("userLogin", user.Id.ToString());
                    HttpContext.Session.SetString("role", "user");
                    return RedirectToPage("Index");
                }
                default:
                    Error = 1;
                    return RedirectToPage("Login");
            }
        }
    }
}