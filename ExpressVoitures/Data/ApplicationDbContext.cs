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

            modelBuilder.Entity<ModelViewCar>()
                .Property(c => c.SellingPrice)
                .HasPrecision(18, 2);


            //modelBuilder.Entity<Car>()
            //   .HasOne<CarBrand>(c => c.CarBrand)
            //   .WithMany() // Si nécessaire, spécifiez ici la collection inverse
            //   .HasForeignKey(c => c.CarBrandId);

            //modelBuilder.Entity<Car>()
            //    .HasOne<CarModel>(c => c.CarModel)
            //    .WithMany() // Si nécessaire, spécifiez ici la collection inverse
            //    .HasForeignKey(c => c.CarModelId);

            modelBuilder.Entity<CarRepair>()
                .HasOne<Car>(c => c.Car)
                .WithMany(c => c.CarRepairs) // Collection inverse dans Car pour les réparations
                .HasForeignKey(c => c.IdCar);


        }
        public DbSet<ExpressVoitures.Models.ModelViewCar> ModelViewCar { get; set; } = default!;
    }
}