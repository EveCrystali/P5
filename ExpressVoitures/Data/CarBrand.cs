using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarBrand
    {
        public int Id { get; set; }

        public int CarBrandId { get; set; }

        [DisplayName("Marque")]
        [Required(ErrorMessage = "La marque est obligatoire.")]
        [StringLength(25, ErrorMessage = "Le nom de la marque ne peut pas dépasser 25 caractères.")]
        public string? CarBrandName { get; set; }

        public virtual ICollection<CarModel>? CarModels { get; set; }
    }
}
