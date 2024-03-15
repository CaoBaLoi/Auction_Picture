
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Auction_Picture.Models{
    public class AppDbContext:DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
            .HasOne(u => u.Account)
            .WithOne(a => a.User)
            .HasForeignKey<Account>(u => u.ID);
        }
        public DbSet<Account> Account {set;get;}
        public DbSet<Product> Product {get;set;}
        public DbSet<User> User {get;set;}
    }
}