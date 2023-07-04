using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
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
        [BindProperty] public User UserLogging { get; set; } = default!;
        [BindProperty] public int Error { get; set; }
        
        public void OnGetAsync()
        {
            Users = _context.Users.ToList();
        }
        
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var selectedUser = await _context.Users.SingleOrDefaultAsync(x =>
                x.Password != null && x.Username != null && x.Username.Equals(UserLogging.Username) && x.Password.Equals(UserLogging.Password));
           
            return RedirectToPage("/Admin/StockBorrowManagement");
        }
    }
}