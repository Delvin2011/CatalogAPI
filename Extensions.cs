using Catalog.Domain;
using Catalog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog
{
    public static class Extensions //extends from one type to another, has to be static
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto
            { //thats establishing a contrcat incase of any changes
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate


            };
        }
    }
}
