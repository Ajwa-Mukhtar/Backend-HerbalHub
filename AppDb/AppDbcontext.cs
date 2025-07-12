using HerbalHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HerbalHub.AppDb
{
    public class AppDbcontext : DbContext
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options)
            : base(options)
        {
        }

        public DbSet<User> UsersDetails { get; set; }
        public DbSet<ContactForm> Contacts { get; set; }
        public DbSet<consultation> Consultations { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<CheckoutProduct> CheckoutProducts { get; set; }

        // ✅ Add this method to configure decimal precision for Price
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CheckoutProduct>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // 18 total digits, 2 after decimal
        }
    }
}
