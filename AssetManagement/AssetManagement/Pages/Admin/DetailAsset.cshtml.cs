using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Pages.Admin
{
    public class DetailAssetModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
        private static String USER = "username";
        public DetailAssetModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Asset asset { get; set; }
        [BindProperty]
        public List<BorrowingAsset> assetBorrowing { get; set; }
        public void OnGet(int id)
        {
            asset = _context.Assets
                .Include(b => b.Category)
                .Where(x => x.Id == id)
                .FirstOrDefault();
            assetBorrowing = _context.BorrowingAssets
                .Include(b => b.Asset)
                .Include(b => b.Borrower)
                .Include(b => b.Asset.Category)
                .Where(x => x.AssetId == id)
                .ToList();

        }
    }
}
