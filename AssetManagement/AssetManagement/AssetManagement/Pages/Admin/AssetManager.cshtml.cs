using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Pages.Admin
{
    public class AssetManagerModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
        private static String USER = "username";
        public AssetManagerModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }
        [BindProperty]
        public FilterAsset Filter { get; set; }
        [BindProperty]
        public IList<Asset> listAsset { get; set; } = default!;
        [BindProperty]
        public IList<Asset> allAssets { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Filter = new FilterAsset();
            Filter.page = 1;
            Filter.status = 3;
            Filter.keyWord = "";
            listAsset = _context.Assets
            .Include(a => a.Category)
            .Skip(0)
            .Take(3)
            .ToList();
            allAssets = _context.Assets.ToList();
            if (allAssets.Count % 3 == 0)
            {
                ViewData["totalPage"] = allAssets.Count / 3;
            }
            else
            {
                ViewData["totalPage"] = allAssets.Count / 3 + 1;
            }
            ViewData["user"] = getUserLogged();

        }
        public async Task OnPostAsync()
        {
            List<Asset> allAlbum = new List<Asset>();
            if (Filter.status != 3)
            {
                bool status = true;
                if (Filter.status == 0)
                {
                    status = false;
                }
                if (Filter.keyWord != null)
                {
                    listAsset = _context.Assets
                .Include(a => a.Category)
                .Where(x => x.Status == status)
                .Where(x => x.AssetName.Contains(Filter.keyWord))
                .Skip((Filter.page - 1) * 3)
                .Take(3)
                .ToList();
                    allAlbum = _context.Assets
                        .Where(x => x.Status == status)
                        .Where(x => x.AssetName.Contains(Filter.keyWord))
                        .ToList();
                }
                else
                {
                    listAsset = _context.Assets
                .Include(a => a.Category)
                .Where(x => x.Status == status)
                .Skip((Filter.page - 1) * 3)
                .Take(3)
                .ToList();

                    allAlbum = _context.Assets
                        .Where(x => x.Status == status)
                        .ToList();
                }
            }
            else
            {
                if (Filter.keyWord != null)
                {
                    listAsset = _context.Assets
                .Include(a => a.Category)
                .Where(x => x.AssetName.Contains(Filter.keyWord))
                .Skip((Filter.page - 1) * 3)
                .Take(3)
                .ToList();

                    allAlbum = _context.Assets
                        .Where(x => x.AssetName.Contains(Filter.keyWord))
                        .ToList();
                }
                else
                {
                    listAsset = _context.Assets
                .Include(a => a.Category)
                .Skip((Filter.page - 1) * 3)
                .Take(3)
                .ToList();

                    allAlbum = _context.Assets
                        .ToList();
                }

            }
            if (allAlbum.Count % 3 == 0)
            {
                ViewData["totalPage"] = allAlbum.Count() / 3;
            }
            else
            {
                ViewData["totalPage"] = allAlbum.Count() / 3 + 1;
            }
            ViewData["user"] = getUserLogged();
        }

        User getUserLogged()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(USER);
            User user = _context.Users.Where(u => u.Username.Equals(jsoncart)).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
