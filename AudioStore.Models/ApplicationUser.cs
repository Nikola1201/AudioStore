
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
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? StreetAddress { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [Range(10000, 99999)]
        public int PostalCode { get; set; }

    }
}
