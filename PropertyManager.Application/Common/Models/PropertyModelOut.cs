using PropertyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Common.Models
{
    public class PropertyModelOut
    {
        public int IdProperty { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public decimal Price { get; set; }

        public string? CodeInternal { get; set; }
        public int Year { get; set; }

        public OwnerModelOut? Owner { get; set; } 
        public List<PropertyTraceModelOut> PropertyTraces { get; set; } = new();
    }
}
