using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Dtos
{
    public record ItemDto
    {
        public Guid Id { get; init; } //Init - great feature for initialisation (only allow setting a value durinf initialisation - make it immutable.
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; set; }

    }

}
