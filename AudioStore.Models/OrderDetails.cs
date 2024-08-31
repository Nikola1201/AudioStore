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
        public int ApplicationUserID { get; set; }
        [ForeignKey("ApplicationUserID")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public ICollection<ShoppingCartItem> Carts { get; set; } = new List<ShoppingCartItem>();
    }
}
