using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ExpressVoitures.Data;

namespace ExpressVoitures.Data
{
    public class CarRepair
    {
        public int Id { get; set; }

        public int IdCar { get; set; }

        public Car Car { get; set; }

        [DisplayName("Coût de la réparation")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "La valeur doit être positive.")]
        public decimal RepairCost { get; set; }

        [DisplayName("Description de la réparation")]
        public string? RepairDescription { get; set; }
    }
}
