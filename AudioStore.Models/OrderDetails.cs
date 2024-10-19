using AudioStore.Models.ViewModels;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("ApplicationUserID")]
        [ValidateNever]
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public double OrderTotal { get; set; }
    }
}
