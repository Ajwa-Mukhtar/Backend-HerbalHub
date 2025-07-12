using System.Collections.Generic;

namespace HerbalHub.Models
{
    public class Checkout
    {
        public long Id { get; set; }
        public string FullName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string ShippingAddress { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string PaymentMethod { get; set; } = "";

        // Navigation property: one order has many products
        public List<CheckoutProduct> Products { get; set; } = new List<CheckoutProduct>();
    }
}
