using Catalog.Domain;
using Catalog.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    public class SqlItemsRepository : IItemsRepository
    {
        private readonly CatalogContext _context;

        public SqlItemsRepository(CatalogContext context)
        {
            _context = context;
        }
        public void CreatItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            _context.Items.Add(item);

            _context.SaveChanges();
        }

        public void DeleteItem(Guid id)
        {
            if (id.ToString() == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            _context.Items.Remove(GetItem(id));
            _context.SaveChanges();

        }

        public Item GetItem(Guid id)
        {
            return _context.Items.Where(item => item.Id == id).SingleOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return _context.Items.ToList();
        }

        public void UpdateItem(Item item)
        {
            //if (item == null)
            //{
            //    throw new ArgumentNullException(nameof(item));
            //}
            //_context.Items.Update(item);

            _context.SaveChanges();//flashes down the changes
        }
    }
}
