using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ostrovskyi.UI.Data;
using Ostrovskyi.UI.Entities;
using Cloth.Domain.Entities;

namespace Ostrovskyi.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Kosuhi kosuhi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Kosuhi == null)
            {
                return NotFound();
            }

            var dish = await _context.Kosuhi.FirstOrDefaultAsync(m => m.Id == id);

            if (dish == null)
            {
                return NotFound();
            }
            else
            {
                kosuhi = kosuhi;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Kosuhi == null)
            {
                return NotFound();
            }
            var dish = await _context.Kosuhi.FindAsync(id);

            if (dish != null)
            {
                kosuhi = kosuhi;
                _context.Kosuhi.Remove(kosuhi);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
