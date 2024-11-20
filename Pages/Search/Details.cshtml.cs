using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Data;
using csci340_iseegreen.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;


namespace csci340_iseegreen.Pages_Search
{
    public class DetailsModel : PageModel
    {
        private readonly ISeeGreenContext _context;

        public DetailsModel(ISeeGreenContext context)
        {
            _context = context;
        }

        public List<Taxa> Taxon {get; set;} = default!;

        public List<Lists> SelectList {get; set;} = default!;

        public string Identifier {get; set;} = default!;

        public string Error {get; set;} = default!;

        public int id {get; set;} = 1;

        private async Task RegenerateFrom(string KewID)
        {
            IQueryable<Taxa> taxaIQ =
                from t in _context.Taxa
                    .Include(g => g.Genus)
                    .Include(f => f.Genus!.Family)
                    .Include(c => c.Genus!.Family!.Category)
                select t;
            taxaIQ = taxaIQ
                .Where(s => s.KewID.Equals(KewID));
            Taxon = await taxaIQ.ToListAsync();
            SelectList = [.. _context.Lists];
        }

        public async Task<IActionResult> OnGetAsync(string KewID)
        {
            await RegenerateFrom(KewID);
            Identifier = KewID;
            return Page();
        }

        public async Task<IActionResult> OnPostAddList(string KewID, int? list) {
            //await MakeApiCall();
            if (list == null) {
                Error = "You have to select a list.";
                await RegenerateFrom(KewID);
                Identifier = KewID;
                //Console.WriteLine("Identifier: " + Identifier);
                return Page();
            }

            ListItems item;

            item = await _context.ListItems.SingleOrDefaultAsync(l => l.KewID == KewID && l.ListID == list.Value);

            if (item != null) {
                Error = "This plant is already in that list.";
                await RegenerateFrom(KewID);
                Identifier = KewID;
                return Page();
            }

            item = new ListItems {
                KewID = KewID,
                ListID = list.Value,
                TimeDiscovered = DateTime.Now
            };

            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListItems/Index", new { itemid = list.ToString() });
        }
    }
}
