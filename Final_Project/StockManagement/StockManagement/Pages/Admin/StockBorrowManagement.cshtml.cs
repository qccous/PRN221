using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StockManagement.Models;

namespace StockManagement.Pages.Admin;

public class StockBorrowManagementModel : PageModel
{
    private readonly StockManagementContext _context;
    private readonly IConfiguration _configuration;

    public StockBorrowManagementModel(StockManagementContext context, IConfiguration configuration, string nameSort, string dateSort, string currentSort, List<Asset> assets)
    {
        _context = context;
        _configuration = configuration;
        NameSort = nameSort;
        DateSort = dateSort;
        CurrentSort = currentSort;
        Assets = assets;
    }
    public string NameSort { get; set; }
    public string DateSort { get; set; }
    public string? CurrentFilter { get; set; }
    public string CurrentSort { get; set; }
    public PaginatedList<Asset> Asset { get;set; } = default!;
    
    public List<Models.Asset> Assets { get; set; }

    public Task OnGetAsync(string sortOrder, string? currentFilter,  int? pageIndex, string? txtSearch)
    {
        CurrentSort = sortOrder;
        NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        DateSort = sortOrder == "Date" ? "date_desc" : "Date";

        if (txtSearch != null)
        {
            Assets = _context.Assets.Where(x => x.AssetName != null && x.AssetName.Contains(txtSearch)).ToList();
        }
        else
        {
            Assets = _context.Assets.ToList();
        }

        CurrentFilter = txtSearch;
        return Task.CompletedTask;
    }
}