using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Data;
using csci340_iseegreen.Models;

namespace csci340_iseegreen.Pages.Families
{
    public class IndexModel : PageModel
    {
        private readonly csci340_iseegreen.Data.ISeeGreenContext _context;

        public IndexModel(csci340_iseegreen.Data.ISeeGreenContext context)
        {
            _context = context;
        }

        public IList<csci340_iseegreen.Models.Families> Families { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Families != null)
            {
                Families = await _context.Families.ToListAsync();
            }
        }
    }
}
