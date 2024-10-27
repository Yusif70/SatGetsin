using Advertisement.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Advertisement.Data.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Image> Images { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ad>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Ads)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Ad>()
                .HasOne(a => a.City)
                .WithMany(c => c.Ads)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
