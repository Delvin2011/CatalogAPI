using Catalog.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Catalog.Domain
{
    public static class PrepDb //setup Db and call our migration file . The method will have to be called from the startup class
    {
        public static void Prepopulation(IApplicationBuilder app) //to get the scope of our Db context so as to use to run the migration
        {
            using (var serviceScope = app.ApplicationServices.CreateAsyncScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CatalogContext>());
            }


        }

        public static void SeedData(CatalogContext context) //we are going to pass our db data here
        {
            System.Console.WriteLine("Applying Mirgation ....");
            context.Database.Migrate(); //takes our latest migration file & applies to the db

            if (!context.Items.Any())
            {
                Item newItem = new Item()
                {
                    Id = System.Guid.NewGuid(),
                    Name = "ZebraHandBag",
                    Price = 100,
                    CreatedDate = System.DateTimeOffset.Now

                };
                System.Console.WriteLine("Adding data - seeding");
                context.Items.Add(newItem);
                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Already have data - not seeding");

            }

        }
    }
}
