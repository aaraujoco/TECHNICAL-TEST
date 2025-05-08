using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Common.Models
{
    public class OwnerModel
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Photo { get; set; }

        [Display(Name = "Birthday")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public string? Birthday { get; set; }


    }
}
