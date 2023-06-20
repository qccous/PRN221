using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetManagement.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
        public ChangePasswordModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string oldPass { get; set; }
        [BindProperty]
        public string newPass { get; set; }
        [BindProperty]
        public string checkNewPass { get; set; }
        public void OnGet()
        {

            ViewData["error"] = "";
        }
        public IActionResult OnPost()
        {
            string username = HttpContext.Session.GetString("username");
            User user = _context.Users.Where(x => x.Username.Equals(username)).FirstOrDefault();
            if (oldPass.Equals(user.Password))
            {
                if(newPass.Equals(checkNewPass))
                {
                    user.Password = newPass;
                    _context.Update(user);
                    _context.SaveChanges();
                    TempData["Message"] = "Change password successfully!";
                    return RedirectToPage("Index");
                }
                else
                {
                    ViewData["error"] = "Your new password is not match with other!";
                    return Redirect("ChangePassword");
                }
            }
            else
            {
                ViewData["error"] = "Old password is incorrect!";
                return Redirect("ChangePassword");
            }
        }
    }
}
