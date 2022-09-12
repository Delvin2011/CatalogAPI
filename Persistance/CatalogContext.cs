using Catalog.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Persistance
{
    public class CatalogContext : IdentityDbContext //Initially it was DbContext
    {
        //Define the constructor
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)// gives us a constructor & calls the base constructor with the options passed in
        {

        }
        //Create a representation of our item model in the database. & we use dbset
        public DbSet<Item> Items { get; set; } //we want to represent Item object as a dbset
    }
}
