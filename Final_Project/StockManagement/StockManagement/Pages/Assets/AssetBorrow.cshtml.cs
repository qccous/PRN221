using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Models;

namespace StockManagement.Pages.Assets;

public class AssetBorrow : PageModel
{
    private readonly StockManagementContext _context;
    private readonly IConfiguration _configuration;

    public AssetBorrow(StockManagementContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public string? NameSort { get; set; }
    public string? DateSort { get; set; }
    public string? CurrentFilter { get; set; }
    public string? CurrentSort { get; set; }
    public PaginatedList<BorrowingAsset> BorrowingAsset { get;set; } = default!;

    public async Task OnGetAsync(string? sortOrder, string? currentFilter,
        string? searchString, int? pageIndex)
    {
        var username = HttpContext.Session.GetString("username");
        if(username != null)
        {
            CurrentSort = sortOrder;
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : string.Empty;
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            var studentsIq = from s in _context.BorrowingAssets.Include(x => x.Asset).Include(x => x.Borrower)
                    .Where(x => x.Borrower != null && x.Borrower.Username != null && x.Borrower != null && x.Borrower.Username.Contains(username)).OrderBy(x => x.Id)
                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIq = studentsIq.Where(s => s.Asset != null && s.Asset.AssetName != null && s.Asset != null && s.Asset.AssetName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentsIq = studentsIq.OrderByDescending(s => s.Asset!.AssetName);
                    break;

                case "Date":
                    studentsIq = studentsIq.OrderBy(s => s.BorrowDate);
                    break;

                case "date_desc":
                    studentsIq = studentsIq.OrderByDescending(s => s.BorrowDate);
                    break;

                default:
                    studentsIq = studentsIq.OrderBy(s => s.Asset!.AssetName);
                    break;
            }

            const int pageSize = 3;
            BorrowingAsset = await PaginatedList<BorrowingAsset>.CreateAsync(studentsIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
		
    }
}