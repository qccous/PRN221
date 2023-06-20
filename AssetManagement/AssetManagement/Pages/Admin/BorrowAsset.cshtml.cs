using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Pages.Admin
{
    public class BorrowAssetModel : PageModel
    {
        private readonly StockManagemnetContext _context;

        public BorrowAssetModel(StockManagemnetContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int AssetId { get; set; }

        public List<User> Users { get; set; }

        public void OnGet(int assetId)
        {
            AssetId = assetId;
            Users = _context.Users.ToList();
        }

        public IActionResult OnPost(int userId, DateTime dueDate)
        {
            
            var borrowingAsset = new BorrowingAsset
            {
                BorrowDate = DateTime.Now,
                AssetId = AssetId,
                Status = true,
                Amount = 1,
                DueDate = dueDate,
                BorrowerId = userId
            };
            var asset = _context.Assets.Find(AssetId);
            asset.Status = true;
            _context.SaveChanges();
            _context.BorrowingAssets.Add(borrowingAsset);
            _context.SaveChanges();

            return RedirectToPage("/Admin/AssetBorrowing/AssetBorrowingManagement");
        }
    }
}
