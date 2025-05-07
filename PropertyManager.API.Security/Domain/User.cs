using Microsoft.AspNetCore.Identity;

namespace PropertyManager.API.Security.Domain
{
    public class User : IdentityUser
    {
        public DateTime Birthday { get; set; }
    }
}
