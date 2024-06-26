using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AudioStore.Models
{
    public class Manufacturer
    {
        [Key]
        public int ManufacturerID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}