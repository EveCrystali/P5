using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class Car
    {
        public int Id { get; set; }

        [DisplayName("Modèle")]
        [Required(ErrorMessage = "Le nom du modèle est obligatoire.")]
        public string Model { get; set; }

        [DisplayName("Marque")]
        [Required(ErrorMessage = "La marque est obligatoire.")]
        public string Brand { get; set; }

        [DisplayName("Finition")]
        public string? Trim { get; set; }

        [DisplayName("Année")]
        [Required(ErrorMessage = "L'année du véhicule est obligatoire.")]
        public int Year { get; set; }

        [DisplayName("Kilométrage")]
        public int Mileage { get; set; }

        [DisplayName("Coût de réparation")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal RepairCost { get; set; }

        [DisplayName("Description des réparations")]
        public string? RepairDescription { get; set; }

        [DisplayName("Prix d'achat")]
        [Required(ErrorMessage = "Le prix d'achat du véhicule est obligatoire.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal PurchasePrice { get; set; }

        [DisplayName("Prix de vente")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal SellingPrice { get; set; }

        [DisplayName("Disponibilité")]
        [Required(ErrorMessage = "La disponibilité du veicule est obligatoire.")]
        public bool Available { get; set; }

        [DisplayName("Date d'achat")]
        public DateOnly PurchaseDate { get; set; }

        [DisplayName("Date de disponibilité à la vente")]
        public DateOnly DateOfAvailability { get; set; }

        [DisplayName("Date de vente")]
        public DateOnly? SaleDate { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("Chemin des images")]
        public List<String> ImagePaths { get; set; }
    }
}