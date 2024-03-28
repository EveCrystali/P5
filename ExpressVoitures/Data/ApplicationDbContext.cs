using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;

namespace ExpressVoitures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ExpressVoitures.Data.Car> Car { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .Property(c => c.RepairCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Car>()
                .Property(c => c.PurchasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Car>()
                .Property(c => c.SellingPrice)
                .HasPrecision(18, 2);

        }

    }
}
