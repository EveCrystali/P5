using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarRepair
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }

        [Required]
        [DisplayName("Coût de la réparation")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal? RepairCost { get; set; }

        [Required]
        [DisplayName("Description de la réparation")]
        [StringLength(40, ErrorMessage = "La description de la réparation ne doit pas depasser 40 caractères.")]
        public string? RepairDescription { get; set; }
    }
}
