using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<ExpressVoitures.Data.Car> Car { get; set; } = default!;

        public DbSet<ExpressVoitures.Data.CarBrand> CarBrand { get; set; } = default!;

        public DbSet<ExpressVoitures.Data.CarModel> CarModel { get; set; } = default!;

        public DbSet<ExpressVoitures.Data.CarMotor> CarMotor { get; set; } = default!;
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
                .WithMany(c => c.CarRepairs)
                .HasForeignKey(c => c.CarId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.CarBrand)
                .WithMany()
                .HasForeignKey(c => c.CarBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.CarModel)
                .WithMany()
                .HasForeignKey(c => c.CarModelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.CarMotor)
                .WithMany()
                .HasForeignKey(c => c.CarMotorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
               .HasOne(c => c.CarTrim)
               .WithMany()
               .HasForeignKey(c => c.CarTrimId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
