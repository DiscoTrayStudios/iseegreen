using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Models;

namespace csci340_iseegreen.Data
{
    public class ISeeGreenContext : IdentityDbContext<IdentityUser>
    {
        public ISeeGreenContext(DbContextOptions<ISeeGreenContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<csci340_iseegreen.Models.Categories>().ToTable("Categories");
            modelBuilder.Entity<csci340_iseegreen.Models.TaxonomicOrders>().ToTable("TaxonomicOrders");
            modelBuilder.Entity<csci340_iseegreen.Models.Families>().ToTable("Families");
            modelBuilder.Entity<csci340_iseegreen.Models.Genera>().ToTable("Genera");
            modelBuilder.Entity<csci340_iseegreen.Models.Taxa>().ToTable("Taxa");
            modelBuilder.Entity<csci340_iseegreen.Models.Synonyms>().ToTable("Synonyms");
            modelBuilder.Entity<IdentityUser>().ToTable("ISeeGreenUsers");
        }

        public DbSet<csci340_iseegreen.Models.Categories> Categories { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.TaxonomicOrders> TaxonomicOrders { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.Families> Families { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.Genera> Genera { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.Taxa> Taxa { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.Synonyms> Synonyms { get; set; } = default!;
        public DbSet<IdentityUser> ISeeGreenUsers { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.Lists> Lists { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.Locations> Locations { get; set; } = default!;
        public DbSet<csci340_iseegreen.Models.ListItems> ListItems { get; set; } = default!;
    }
}
