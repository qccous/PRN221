using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Models;

namespace AssetManagement.Pages.Admin.AssetBorrowing
{
    public class AssetBorrowingManagementModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
        private readonly IConfiguration _configuration;

        public AssetBorrowingManagementModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Asset> Asset { get;set; } = default!;

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

            IQueryable<Asset> studentsIQ = from s in _context.Assets.Include(x => x.Category).OrderBy(x => x.Id)
                                                    select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.AssetName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.AssetName);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.AssetName);
                    break;
            }

            var pageSize = 3;
            Asset = await PaginatedList<Asset>.CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
