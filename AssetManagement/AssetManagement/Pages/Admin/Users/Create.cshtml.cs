using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AssetManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Pages.Admin.Users
{
    public class CreateModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;

        public CreateModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["RoleName"] = new SelectList(_context.Roles, "RoleName", "RoleName");
            return Page();
        }
        public string Message { get; set; }
        [BindProperty]
        public int role { get; set; }
        [BindProperty]
        public User User { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Users == null || User == null)
            {
                return Page();
            }
            var userExists = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == User.Username);
            if (userExists == null)
            {
                User.RoleId = role;
                _context.Users.Add(User);
                await _context.SaveChangesAsync();
            }
            else
            {
                Message = "Username is exised";
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
