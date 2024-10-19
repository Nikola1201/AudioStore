using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Models.ViewModels
{
    public class OrderVM
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public double OrderTotal { get; set; }
    }
}
