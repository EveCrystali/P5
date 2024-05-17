using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarRepair
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }

        [DisplayName("Coût de la réparation")]
        [RegularExpression(@"^\d+([.,]\d{0,2})?$", ErrorMessage = "Le coût de réparation doit être un nombre avec au maximum deux chiffres apres le séparateur décimal.")]
        [Required(ErrorMessage = "Le coût de la réparation est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal? RepairCost { get; set; }

        [Required(ErrorMessage = "La description de la réparation est obligatoire.")]
        [DisplayName("Description de la réparation")]
        [StringLength(40, ErrorMessage = "La description de la réparation ne doit pas depasser 40 caractères.")]
        public string? RepairDescription { get; set; }
    }
}
