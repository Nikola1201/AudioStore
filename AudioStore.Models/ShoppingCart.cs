using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Models
{
    public class ShoppingCartItem
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product Product { get; set; }
        public int OrderDetailsID { get; set; }
        public OrderDetails OrderDetails { get; set; }
        [Range(1, 1000, ErrorMessage = "Please enter a value betweem 1 and 1000")]
        public int Count { get; set; }
        public double Price { get; set; }
        public double Total { get=> this.Count * this.Price; }
    }
}
