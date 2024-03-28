using System.ComponentModel.DataAnnotations;
using ExpressVoitures.Data;

namespace ExpressVoitures.Models
{
    public class ModeleVueCar
    {
        public int IdCar { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }

        public string Trim { get; set; }

        public int Year { get; set; }
        public int Mileage { get; set; }

        public decimal SellingPrice { get; set; }
        public DateOnly DateOfAvailability { get; set; }

        public string? Description { get; set; }
    }
}