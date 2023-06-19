using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Models;

namespace AssetManagement.Pages.Assets
{
    public class AssetBorrowingModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;
        private static String USER = "userlogin";
        public AssetBorrowingModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }

        public IList<BorrowingAsset> BorrowingAsset { get; set; } = default!;

        public async Task OnGetAsync(int? page, String txtSearch)
        {
            if (_context.BorrowingAssets != null)
            {
                User user = getUserLogged();
                if (user != null)
                {
                    if (page <= 0)
                    {
                        page = 1;
                    }
                    if (BorrowingAsset == null)
                    {
                        BorrowingAsset = await _context.BorrowingAssets
                        .Include(b => b.Asset)
                        .Include(b => b.Borrower)
                        .Include(b => b.Asset.Category)
                        .Where(b => b.BorrowerId == user.Id)
                        .ToListAsync();
                    }
                    ViewData["user"] = user;
                    ViewData["page"] = 1;
                    ViewData["status"] = 0;
                    if (BorrowingAsset.Count % 3 == 0)
                    {
                        ViewData["totalPage"] = BorrowingAsset.Count() / 3;
                    }
                    else
                    {
                        ViewData["totalPage"] = BorrowingAsset.Count() / 3 + 1;
                    }

                    BorrowingAsset = await _context.BorrowingAssets
                   .Include(b => b.Asset)
                   .Include(b => b.Borrower)
                   .Include(b => b.Asset.Category)
                   .Where(b => b.BorrowerId == user.Id)
                   .Skip(0)
                   .Take(3)
                   .ToListAsync();
                }
            }
        }
        //public async Task OnPostAsync(String stat, int? page)
        //{
        //    if (_context.BorrowingAssets != null)
        //    {
        //        User user = getUserLogged();
        //        if (user != null)
        //        {
        //            bool status = true;
        //            if (stat != null)
        //            {
        //                if (stat.Equals("1"))
        //                {
        //                    status = true;
        //                }
        //                else
        //                {
        //                    status = false;
        //                }

        //                if (stat.Equals("3") || stat == null)
        //                {
        //                    if (page == null)
        //                    {
        //                        page = 1;
        //                    }

        //                    BorrowingAsset = await _context.BorrowingAssets
        //                .Include(b => b.Asset)
        //                .Include(b => b.Borrower)
        //                .Include(b => b.Asset.Category)
        //                .Skip((int)((page - 1) * 3))
        //                .Take(3)
        //                .Where(b => b.BorrowerId == user.Id)
        //                .ToListAsync();

        //                }


        //                page = 1;
        //                BorrowingAsset = await _context.BorrowingAssets
        //               .Include(b => b.Asset)
        //               .Include(b => b.Borrower)
        //               .Include(b => b.Asset.Category)
        //               .Where(b => b.BorrowerId == user.Id && b.Status == status)
        //               .ToListAsync();
        //                if (stat.Equals("3"))
        //                {
        //                    ViewData["status"] = null;

        //                }
        //                else
        //                {
        //                    ViewData["status"] = status;
        //                }
        //                ViewData["page"] = page;
        //                if (BorrowingAsset.Count % 3 == 0)
        //                {
        //                    ViewData["totalPage"] = BorrowingAsset.Count() / 3;
        //                }
        //                else
        //                {
        //                    ViewData["totalPage"] = BorrowingAsset.Count() / 3 + 1;
        //                }
        //            }
        //            else
        //            {
        //                BorrowingAsset = await _context.BorrowingAssets
        //            .Include(b => b.Asset)
        //            .Include(b => b.Borrower)
        //            .Include(b => b.Asset.Category)
        //            .Skip((int)((page - 1) * 3))
        //            .Take(3)
        //            .Where(b => b.BorrowerId == user.Id)
        //            .ToListAsync();
        //                ViewData["user"] = user;
        //            }

        //            ViewData["user"] = user;
        //            ViewData["page"] = page;

        //            var BorrowingAsset2 = await _context.BorrowingAssets
        //            .Include(b => b.Asset)
        //            .Include(b => b.Borrower)
        //            .Include(b => b.Asset.Category)
        //            .Where(b => b.BorrowerId == user.Id)
        //            .ToListAsync();

        //            if (BorrowingAsset2.Count % 3 == 0)
        //            {
        //                ViewData["totalPage"] = BorrowingAsset2.Count() / 3;
        //            }
        //            else
        //            {
        //                ViewData["totalPage"] = BorrowingAsset2.Count() / 3 + 1;
        //            }
        //        }

        //    }
        //}

        public async Task OnPostAsync(String stat, int? page)
        {
            if (_context.BorrowingAssets != null)
            {
                User user = getUserLogged();
                if (user != null)
                {
                    if (stat.Equals("0"))
                    {
                        BorrowingAsset = await _context.BorrowingAssets
                        .Include(b => b.Asset)
                        .Include(b => b.Borrower)
                        .Include(b => b.Asset.Category)
                        .Skip((int)((page - 1) * 3))
                        .Take(3)
                        .Where(b => b.BorrowerId == user.Id)
                        .ToListAsync();

                        var allBorrowingAsset = await _context.BorrowingAssets
                         .Include(b => b.Asset)
                        .Include(b => b.Borrower)
                          .Include(b => b.Asset.Category)
                          .Where(b => b.BorrowerId == user.Id)
                    .ToListAsync();
                             //Total
                        if (allBorrowingAsset.Count % 3 == 0)
                        {
                            ViewData["totalPage"] = allBorrowingAsset.Count() / 3;
                        }
                        else
                        {
                            ViewData["totalPage"] = allBorrowingAsset.Count() / 3 + 1;
                        }
                        ViewData["page"] = page;
                        ViewData["user"] = user;
                        ViewData["status"] = 0;
                    }
                    else
                    {
                        bool status;
                        int stats;
                        if (stat.Equals("1"))
                        {
                            status = true;
                            stats = 1;
                        }
                        else
                        {
                            status = false;
                            stats = 2;
                        }
                        BorrowingAsset = await _context.BorrowingAssets
                        .Include(b => b.Asset)
                        .Include(b => b.Borrower)
                        .Include(b => b.Asset.Category)
                        .Skip((int)((page - 1) * 3))
                        //.Take(3)
                        .Where(b => b   .BorrowerId == user.Id && b.Status == status)
                        .Take(3)
                        .ToListAsync();


                        var allBorrowingAsset = await _context.BorrowingAssets
                        .Include(b => b.Asset)
                        .Include(b => b.Borrower)
                        .Include(b => b.Asset.Category)
                        .Where(b => b.BorrowerId == user.Id && b.Status == status)
                         .ToListAsync();

                        ViewData["page"] = page;
                        ViewData["user"] = user;
                        ViewData["status"] = stats;
                        if (allBorrowingAsset.Count % 3 == 0)
                        {
                            ViewData["totalPage"] = allBorrowingAsset.Count() / 3;
                        }
                        else
                        {
                            ViewData["totalPage"] = allBorrowingAsset.Count() / 3 + 1;
                        }
                    }

                }
            }
        }

        User getUserLogged()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(USER);
            User user = _context.Users.Where(u => u.Id == int.Parse(jsoncart)).FirstOrDefault();
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
