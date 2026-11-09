using Microsoft.EntityFrameworkCore;

namespace abibas_back.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserItem> UserItems { get; set; }
        public DbSet<AddressItem> AddressItems { get; set; }
        public DbSet<CommentItem> CommentItems { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<WishlistItem> WhishListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // composite pk 
            modelBuilder.Entity<WishlistItem>().HasKey(w => new {w.UserId, w.ProductId});
        }
    }
}
