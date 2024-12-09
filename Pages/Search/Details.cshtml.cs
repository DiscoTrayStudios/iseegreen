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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;


namespace csci340_iseegreen.Pages_Search
{
    public class DetailsModel : PageModel
    {
        private readonly ISeeGreenContext _context;

        public DetailsModel(ISeeGreenContext context)
        {
            _context = context;
        }

        public Taxa Taxon {get; set;} = default!;

        public List<Lists> SelectList {get; set;} = default!;

        public string Error {get; set;} = default!;

        private async Task<IActionResult> RegenerateFrom(string KewID)
        {
            SelectList = [.. _context.Lists];
            IQueryable<Taxa> taxaIQ =
                from t in _context.Taxa
                    .Include(g => g.Genus)
                    .Include(f => f.Genus!.Family)
                    .Include(c => c.Genus!.Family!.Category)
                select t;
            taxaIQ = taxaIQ
                .Where(s => s.KewID.Equals(KewID));
            Taxa? maybeTaxon = taxaIQ.FirstOrDefault();
            if (maybeTaxon == null)
            {
                Error = "This plant doesn't have a valid Kew ID.";
                return Page();
            }
            Taxon = maybeTaxon!;
            return Page();
        }

        public async Task<IActionResult> OnGetAsync(string KewID)
        {
            return await RegenerateFrom(KewID);
        }

        public async Task<IActionResult> OnPostAddList(string KewID, int? list, string? userID, string? newName) {
            int? receivingListID;
            Console.WriteLine("Starting OnPostAddList...");
            Console.WriteLine($"KewID = '{KewID}'; list = {list}; userID = '{userID}'; newName = '{newName}'");
            if (list == null)
            {
                Console.WriteLine("list was null");
                if (newName == null)
                {
                    Console.WriteLine("newName was null");
                    // The user hasn't selected an existing list or given a name for a new list.
                    Error = "You have to select a list.";
                    return await RegenerateFrom(KewID);
                }
                else
                {
                    Console.WriteLine("newName was not null");
                    // TODO we might need to ensure that the given name is unique for this user; not sure at the moment.
                    // Generate a unique ID for the new list.
                    HashSet<int> existingListIDs = [.. _context.Lists.Select(l => l.Id)];
                    bool doContinue = true;
                    int newListID = 0;
                    while (doContinue)
                    {
                        newListID = new Random().Next();
                        doContinue = existingListIDs.Contains(newListID);
                    }
                    // Create a new list.
                    Lists newUserList = new()
                    {
                        Name = newName,
                        Id = newListID,
                        OwnerID = userID!,
                    };
                    _context.Lists.Add(newUserList);
                    _context.SaveChanges();
                    receivingListID = newListID;
                }
            }
            else
            {
                Console.WriteLine("list was not null");
                // Check if the plant is already in the given list.
                IQueryable<ListItems> matches =
                    from li in _context.ListItems
                        .Where(li => li.ListID == list && li.KewID == KewID)
                    select li;
                if (!matches.IsNullOrEmpty())
                {
                    Console.WriteLine($"The given KewID is already in the list with ID = {list}");
                    // The given plant is already in the selected list.
                    Error = "This plant is already in the selected list.";
                    return await RegenerateFrom(KewID);
                }
                receivingListID = list;
            }
            // Create and add the new ListItem.
            ListItems newItem = new()
            {
                KewID = KewID,
                ListID = receivingListID.Value,
                TimeDiscovered = DateTime.Now,
            };

            _context.ListItems.Add(newItem);
            _context.SaveChanges();

            Console.WriteLine("Finished OnPostAddList");
            return await RegenerateFrom(KewID);
        }
    }
}
