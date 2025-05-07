using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Common.Models
{
    public class OwnerModelOut
    {
        public int IdOwner { get; set; }
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Photo { get; set; }

        public string? Birthday { get; set; }


    }
}
