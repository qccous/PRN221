using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Pages.Admin.AssetBorrowing
{
	public class ReturnAssetModel : PageModel
	{
		private readonly StockManagemnetContext _context;

		public ReturnAssetModel(StockManagemnetContext context)
		{
			_context = context;
		}
		[BindProperty]

		public Asset Asset { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{

			var asset = await _context.Assets.FirstOrDefaultAsync(m => m.Id == id);
			if (asset == null)
			{
				return NotFound();
			}
			Asset = asset;
			return Page();
		}
		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				var asset = await _context.Assets.FirstOrDefaultAsync(m => m.Id == Asset.Id);
				asset.Status = false;

				var borrowingAsset = _context.BorrowingAssets.Where(x => x.AssetId == Asset.Id).FirstOrDefault();
				borrowingAsset.Status = false;
				borrowingAsset.RetrurnDate = DateTime.Now;

				_context.ChangeTracker.Clear();
				_context.BorrowingAssets.Update(borrowingAsset);
				_context.Assets.Update(asset);

				_context.SaveChanges();



			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(Asset.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("/Admin/AssetBorrowing/AssetBorrowingManagement");
		}
		private bool OrderExists(int id)
		{
			return (_context.Assets?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}

}
