using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Data;
using csci340_iseegreen.Models;

namespace csci340_iseegreen.Pages.TaxonomicOrders
{
    public class DeleteModel : PageModel
    {
        private readonly csci340_iseegreen.Data.ISeeGreenContext _context;

        public DeleteModel(csci340_iseegreen.Data.ISeeGreenContext context)
        {
            _context = context;
        }

        [BindProperty]
        public csci340_iseegreen.Models.TaxonomicOrders TaxonomicOrders { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.TaxonomicOrders == null)
            {
                return NotFound();
            }

            var taxonomicorders = await _context.TaxonomicOrders.FirstOrDefaultAsync(m => m.TaxonomicOrder == id);

            if (taxonomicorders == null)
            {
                return NotFound();
            }
            else
            {
                TaxonomicOrders = taxonomicorders;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.TaxonomicOrders == null)
            {
                return NotFound();
            }
            var taxonomicorders = await _context.TaxonomicOrders.FindAsync(id);

            if (taxonomicorders != null)
            {
                TaxonomicOrders = taxonomicorders;
                _context.TaxonomicOrders.Remove(TaxonomicOrders);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
