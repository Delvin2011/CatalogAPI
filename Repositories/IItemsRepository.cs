using Catalog.Domain;
using System;
using System.Collections.Generic;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();

        void CreatItem(Item item); //receives item that needs to be created in the repository. Don't return nothing.

        void UpdateItem(Item item);

        void DeleteItem(Guid id);
    }
}