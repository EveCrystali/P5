using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressVoitures.Data;

namespace ExpressVoitures.Models
{
    public class ModelViewCar
    {
        public int Id { get; set; }
        public int IdCar { get; set; }

        public int IdCarBrand { get; set; }

        public int IdCarModel { get; set; }

        public bool IsAvailable { get; set; }

        public string CarModel { get; set; }

        public string CarBrand { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }

        public decimal SellingPrice { get; set; }

        public DateOnly DateOfAvailability { get; set; }

        public string? Description { get; set; }
    }
}