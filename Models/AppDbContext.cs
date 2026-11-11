using Microsoft.EntityFrameworkCore;

namespace abibas_back.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserItem> UserItems { get; set; }
        public DbSet<CommentItem> CommentItems { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<TechnicalDetailItem> TechnicalDetailItems { get; set; }
        public DbSet<CheckoutItem> CheckoutItems { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

    }
}
