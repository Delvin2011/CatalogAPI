using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Domain
{
public record Item
    {
        [Key]
        public Guid Id { get; init; } //Init - great feature for initialisation (only allow setting a value durinf initialisation - make it immutable.
        [Required]
        [MaxLength(250)]
        public string Name { get; init; }
        [Required]
        public decimal Price { get; init; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }

    }
    
}
