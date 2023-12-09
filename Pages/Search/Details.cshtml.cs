using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Data;
using csci340_iseegreen.Models;

namespace csci340_iseegreen.Pages_Search
{
    public class DetailsModel : PageModel
    {
        private readonly csci340_iseegreen.Data.ISeeGreenContext _context;

        public DetailsModel(csci340_iseegreen.Data.ISeeGreenContext context)
        {
            _context = context;
        }

        public List<Taxa> Taxon {get; set;} = default!;

        public List<Lists> SelectList {get; set;} = default!;

        public async Task<IActionResult> OnGetAsync(string KewID)
        {
            IQueryable<csci340_iseegreen.Models.Taxa> taxaIQ = from t in _context.Taxa.Include(g => g.Genus).Include(f => f.Genus!.Family).Include(c => c.Genus!.Family!.Category) select t;

            taxaIQ = taxaIQ.Where(s => s.KewID.Equals(KewID));

            var taxon = await taxaIQ.ToListAsync();

            if (taxon == null) {
                return NotFound();
            }
            else {
                Taxon = taxon;
            }

            if (_context.Lists != null)
            {
                SelectList = await _context.Lists
                .ToListAsync();
            }
            return Page();
        }
    }
}
