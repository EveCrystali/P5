using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using ExpressVoitures.Models;

namespace ExpressVoitures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ExpressVoitures.Data.Car> Car { get; set; } = default!;

        public DbSet<ExpressVoitures.Data.CarBrand> CarBrand { get; set; } = default!;

        public DbSet<ExpressVoitures.Data.CarModel> CarModel { get; set; } = default!;

        public DbSet<ExpressVoitures.Data.CarRepair> CarRepair { get; set; } = default!;

        public DbSet<ExpressVoitures.Data.CarTrim> CarTrim { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarRepair>()
                .Property(c => c.RepairCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Car>()
                .Property(c => c.PurchasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Car>()
                .Property(c => c.SellingPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CarRepair>()
                .HasOne<Car>(c => c.Car)
                .WithMany(c => c.CarRepairs) // Collection inverse dans Car pour les réparations
                .HasForeignKey(c => c.CarId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.CarBrand)
                .WithMany()
                .HasForeignKey(c => c.CarBrandId)
                .OnDelete(DeleteBehavior.Restrict); // ou DeleteBehavior.SetNull

            modelBuilder.Entity<Car>()
                .HasOne(c => c.CarModel)
                .WithMany()
                .HasForeignKey(c => c.CarModelId)
                .OnDelete(DeleteBehavior.Restrict); // ou DeleteBehavior.SetNull

            modelBuilder.Entity<Car>()
               .HasOne(c => c.CarTrim)
               .WithMany()
               .HasForeignKey(c => c.CarTrimId)
               .OnDelete(DeleteBehavior.Restrict); // ou DeleteBehavior.SetNull

        }

    }
}