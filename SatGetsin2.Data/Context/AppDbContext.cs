using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SatGetsin2.Core.Entities;

namespace SatGetsin2.Data.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ad>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Ads)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Ad>()
                .HasOne(a => a.City)
                .WithMany(c => c.Ads)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Ad>()
                .HasOne(a => a.User)
                .WithMany(u => u.Ads)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AppUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var ads = ChangeTracker.Entries<Ad>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added)
                .Select(e => e.Entity);
            foreach (var ad in ads)
            {
                if (ad.ExpireDate < DateTime.Now && !ad.IsDeleted)
                {
                    ad.IsDeleted = true;
                    Notification notification = new()
                    {
                        AdId = ad.Id,
                        Message = "Your ad expired",
                        CreatedAt = DateTime.Now,
                    };
                    await Set<Notification>().AddAsync(notification, cancellationToken);
                }
            }
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
