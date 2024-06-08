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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Kosuhi kosuhi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Kosuhi == null)
            {
                return NotFound();
            }

            var kosuhi = await _context.Kosuhi.FirstOrDefaultAsync(m => m.Id == id);
            if (kosuhi == null)
            {
                return NotFound();
            }
            else
            {
                kosuhi = kosuhi;
            }
            return Page();
        }
    }
}