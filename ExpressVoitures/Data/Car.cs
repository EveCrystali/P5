using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Data
{
    public class Car
    {
        public CarBrand? CarBrand { get; set; }
        public int CarBrandId { get; set; }
        public CarModel? CarModel { get; set; }
        public int CarModelId { get; set; }
        public CarMotor? CarMotor { get; set; }
        public int? CarMotorId { get; set; }
        public List<CarRepair>? CarRepairs { get; set; }
        public CarTrim? CarTrim { get; set; }
        public int CarTrimId { get; set; }
        public string? CodeVIN { get; set; }
        [DisplayName("Date de disponibilité à la vente")]
        public DateOnly? DateOfAvailability { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        public int Id { get; set; }
        [DisplayName("Chemin des images")]
        public List<string>? ImagePaths { get; set; }

        [NotMapped]
        public List<IFormFile>? Images { get; set; }

        [DisplayName("Disponibile")]
        [Required(ErrorMessage = "La disponibilité du vehicule est obligatoire.")]
        public bool IsAvailable { get; set; }

        [DisplayName("Kilométrage")]
        public int? Mileage { get; set; }

        [DisplayName("Date d'achat")]
        public DateOnly? PurchaseDate { get; set; }

        [DisplayName("Prix d'achat")]
        [Required(ErrorMessage = "Le prix d'achat du véhicule est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal? PurchasePrice { get; set; }

        [DisplayName("Date de vente")]
        public DateOnly? SaleDate { get; set; }

        [DisplayName("Prix de vente")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal? SellingPrice { get; set; }

        [DisplayName("Année")]
        [Required(ErrorMessage = "L'année du véhicule est obligatoire.")]
        public int? Year { get; set; }
    }
}
