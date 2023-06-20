using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Models;

namespace AssetManagement.Pages.Admin.AssetBorrowing
{
    public class ListModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
        private readonly IConfiguration _configuration;

        public ListModel(AssetManagement.Models.StockManagemnetContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<BorrowingAsset> BorrowingAsset { get;set; } = default!;

        public async Task OnGetAsync(string sortOrder, string currentFilter,
             string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

            IQueryable<BorrowingAsset> studentsIQ = from s in _context.BorrowingAssets.Include(x => x.Asset).Include(x=> x.Borrower).OrderBy(x => x.Id)
                                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.Borrower.Username.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.Asset.AssetName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.BorrowDate);
                    break;

                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.BorrowDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.Asset.AssetName);
                    break;
            }

            var pageSize = 3;
            BorrowingAsset = await PaginatedList<BorrowingAsset>.CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
