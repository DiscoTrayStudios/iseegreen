using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using csci340_iseegreen.Data;
using csci340_iseegreen.Models;

namespace csci340_iseegreen.Pages.TaxonomicOrders
{
    public class CreateModel : PageModel
    {
        private readonly csci340_iseegreen.Data.ISeeGreenContext _context;

        public CreateModel(csci340_iseegreen.Data.ISeeGreenContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public csci340_iseegreen.Models.TaxonomicOrders TaxonomicOrders { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.TaxonomicOrders == null || TaxonomicOrders == null)
            {
                return Page();
            }

            _context.TaxonomicOrders.Add(TaxonomicOrders);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
