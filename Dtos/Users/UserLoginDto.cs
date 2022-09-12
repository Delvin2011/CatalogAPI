using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Dtos.Users
{
    public class UserLoginDto
    {

        [Required]
        [EmailAddress] //called annotations
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
