﻿namespace AudioStore.Models.ViewModels
{
    public class OrderDetailEditVM
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string OrderDate { get; set; }
        public OrderStatus SelectedOrderStatus { get; set; }
        public double OrderTotal { get; set; }

        public ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
