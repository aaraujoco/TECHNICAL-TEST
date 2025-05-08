using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Domain.Entities
{
    public class PropertyImage
    {
        [Key]
        public int IdPropertyImage { get; set; }

        [Required]
        public int IdProperty { get; set; }

        [Required]
        public string File { get; set; } = string.Empty;

        public bool Enabled { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(IdProperty))]
        public Property? Property { get; set; }
    }
}
