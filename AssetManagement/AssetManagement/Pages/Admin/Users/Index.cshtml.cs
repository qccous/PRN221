using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Models;

namespace AssetManagement.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
		private readonly IConfiguration _configuration;

		public IndexModel(AssetManagement.Models.StockManagemnetContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}
		public string NameSort { get; set; }
		public string DateSort { get; set; }
		public string CurrentFilter { get; set; }
		public string CurrentSort { get; set; }
		public PaginatedList<User> User { get;set; } = default!;

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

			IQueryable<User> studentsIQ = from s in _context.Users.Include(x => x.Role).OrderBy(x => x.Id)
											 select s;
			if (!String.IsNullOrEmpty(searchString))
			{
				studentsIQ = studentsIQ.Where(s => s.Username.Contains(searchString));
			}

			switch (sortOrder)
			{
				case "name_desc":
					studentsIQ = studentsIQ.OrderByDescending(s => s.Username);
					break;

				default:
					studentsIQ = studentsIQ.OrderBy(s => s.Username);
					break;
			}

			var pageSize = 3;
			User = await PaginatedList<User>.CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
		}
	}
}
