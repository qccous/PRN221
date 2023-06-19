using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Pages
{
    public class EditAccountModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;

        public EditAccountModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;
        [BindProperty]
        public bool male { get; set; } = default!;
        [BindProperty]
        public bool female { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            string username = HttpContext.Session.GetString("username");
            User userTemp = _context.Users.Where(x => x.Username.Equals(username)).FirstOrDefault();
            int id = userTemp.Id;
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string username = HttpContext.Session.GetString("username");
            User userTemp = _context.Users.Where(x => x.Username.Equals(username)).FirstOrDefault();
            userTemp.FirstName = User.FirstName;
            userTemp.LastName = User.LastName;
            userTemp.PhoneNumber = User.PhoneNumber;
            userTemp.Addrress = User.Addrress;
            _context.Update(userTemp);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
