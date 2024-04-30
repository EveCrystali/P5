using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarMotor
    {
        public int Id { get; set; }

        public int CarModelId { get; set; }

        public virtual CarModel? CarModel { get; set; }

        [DisplayName("Motorisation")]
        [StringLength(25, ErrorMessage = "Le nom de la motorisation ne peut pas dépasser 25 caractères.")]
        public string? CarMotorName { get; set; }
    }
}