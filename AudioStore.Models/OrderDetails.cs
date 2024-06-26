using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderID { get; set; }
        public string ApplicationUserID { get; set; }
        [ForeignKey("ApplicationUserID")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

    }
}
