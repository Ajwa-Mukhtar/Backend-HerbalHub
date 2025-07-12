namespace HerbalHub.DTO.UserVM
{
    public class checkoutvm
    {
        public string FullName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string ShippingAddress { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public List<CheckoutProductVM> Products { get; set; } = new();
    }

    public class CheckoutProductVM
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
