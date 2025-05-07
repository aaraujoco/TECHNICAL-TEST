using System.ComponentModel.DataAnnotations;

namespace PropertyManager.API.Security.Domain.Model
{
    public class UserCredentialModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
