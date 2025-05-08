using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PropertyManager.Application.Common.Models
{
    public class UploadPropertyImageModel
    {
        [Required]
        public int IdProperty { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
