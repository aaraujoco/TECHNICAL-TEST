using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Common.Models
{
    public class PropertyTraceModel
    {
        public DateTime DateSale { get; set; }

        /// <summary>
        /// Name associated with the property trace.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Value of the property.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Tax applied to the property.
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Identifier for the associated property.
        /// </summary>
        public int IdProperty { get; set; }

      
    }
}
