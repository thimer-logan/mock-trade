using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StocksApp.Domain.Entities.Identity;
using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Infrastructure.DbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<Watchlist> Watchlist { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Portfolio>().ToTable("Portfolios");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Holding>().ToTable("Holdings");
            modelBuilder.Entity<Watchlist>().ToTable("Watchlists");
            modelBuilder.Entity<WatchlistItem>().ToTable("WatchlistItems");
        }
    }
}
