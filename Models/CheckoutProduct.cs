using System.ComponentModel.DataAnnotations.Schema;

namespace HerbalHub.Models
{
    public class CheckoutProduct
    {
        public long Id { get; set; }

        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Foreign Key
        public long CheckoutId { get; set; }

        // Navigation property (link to parent Checkout)
        [ForeignKey("CheckoutId")]
        public Checkout? Checkout { get; set; }
    }
}
