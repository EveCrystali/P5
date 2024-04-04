using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressVoitures.Data;

namespace ExpressVoitures.Models
{
    public class ModelViewCar
    {
        public int Id { get; set; }
        public int IdCar { get; set; }

        public int CarBrandId { get; set; }

        public int CarModelId { get; set; }

        public int CarTrimId { get; set; }

        public bool IsAvailable { get; set; }

        [DisplayName("Modèle")]
        public string CarModel { get; set; }

        [DisplayName("Marque")]
        public string CarBrand { get; set; }

        [DisplayName("Année")]
        public int Year { get; set; }

        [DisplayName("Kilométrage")]
        public int Mileage { get; set; }

        [DisplayName("Prix de vente")]
        public decimal SellingPrice { get; set; }

        [DisplayName("Date de disponibilité")]
        public DateOnly DateOfAvailability { get; set; }

        public string? Description { get; set; }

        public List<String>? ImagePaths { get; set; }
    }
}