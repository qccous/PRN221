using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AssetManagement.Models;

namespace AssetManagement.Pages.Assets
{
    public class CreateModel : PageModel
    {
        private readonly AssetManagement.Models.StockManagemnetContext _context;

        public CreateModel(AssetManagement.Models.StockManagemnetContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<IFormFile> file { get; set; }
        public IActionResult OnGet()
        {
            ViewData["CategoryName"] = new SelectList(_context.Categories, "CategoryName", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Asset Asset { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Assets == null || Asset == null)
            {
                return Page();
            }
            for (int i = 0; i < file.Count; i++)
            {
                if (file != null && file[i].Length > 0)
                {
                    Asset.Image = file[0].FileName;
                    var fileName = Path.GetFileName(file[i].FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file[i].CopyToAsync(stream);
                    }
                }
            }

            var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            List<String> listFileName = Directory.GetFiles(imageDirectory).Select(Path.GetFileName).ToList();
            ViewData["listFile"] = listFileName;
            Asset.Status = false;
            _context.Assets.Add(Asset);
            await _context.SaveChangesAsync();
            return RedirectToPage("./List");
        }
    }
}