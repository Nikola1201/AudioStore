using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudioStore.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        [ValidateNever]
        public virtual Category Category { get; set; }
        [Required]
        [Display(Name = "Manufacturer")]
        public int ManufacturerID { get; set; }
        [ForeignKey("ManufacturerID")]
        [ValidateNever]
        public Manufacturer Manufacturer { get; set; }

    }
}
