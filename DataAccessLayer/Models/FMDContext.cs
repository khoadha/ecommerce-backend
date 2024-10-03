using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.Models {
    public class EXEContext : IdentityDbContext<ApplicationUser> {
        public EXEContext() { }

        public EXEContext(DbContextOptions<EXEContext> options) : base(options) { }

        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProducts> OrdersProducts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterials { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<FeedbackImage> FeedbackImages { get; set; }
        public virtual DbSet<Bidding> Biddings { get; set; }
        public virtual DbSet<BiddingCategory> BiddingCategories { get; set; }
        public virtual DbSet<BiddingMaterial> BiddingMaterials { get; set; }
        public virtual DbSet<Auctioneer> Auctioneers { get; set; }
        public virtual DbSet<AuctioneerImage> AuctioneerImages { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public virtual DbSet<GlobalSettings> GlobalSettings { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<Billing> Billings { get; set; }
        public virtual DbSet<BillingPackage> BillingPackages { get; set; }
        public virtual DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }

        private string GetConnectionString() {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json", true, true)
                 .Build();

            var strConn = config["ConnectionStrings:DefaultConnection"];

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ApplicationUser>()
        .HasOne(a => a.ManagedStore)
        .WithOne(s => s.Manager)
        .HasForeignKey<ApplicationUser>(a => a.ManagedStoreId)
        .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<OrderProducts>()
        .HasOne(op => op.Product)
        .WithMany(p => p.OrderProducts)
        .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);

        }

    }
}
