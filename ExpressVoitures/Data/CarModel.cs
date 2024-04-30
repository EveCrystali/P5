using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarModel
    {
        public int Id { get; set; }

        public int CarModelId { get; set; }

        public int CarBrandId { get; set; }
        public virtual CarBrand? CarBrand { get; set; }

        [DisplayName("Modèle")]
        [Required(ErrorMessage = "Le nom du modèle est obligatoire.")]
        [StringLength(25, ErrorMessage = "Le nom du modèle ne peut pas dépasser 25 caractères.")]
        public string? CarModelName { get; set; }

        public virtual ICollection<CarTrim>? Trims { get; set; }

        public virtual ICollection<CarMotor>? Motors { get; set; }
    }
}
