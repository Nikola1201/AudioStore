
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Models
{
    public class ApplicationUser
    {
        public int ApplicationUserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public int PostalCode { get; set; }

    }
}
