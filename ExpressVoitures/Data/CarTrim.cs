using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoitures.Data
{
    public class CarTrim
    {
        public int Id { get; set; }

        public int CarModelId { get; set; }

        public virtual CarModel? CarModel { get; set; }

        [DisplayName("Finition")]
        [StringLength(25, ErrorMessage = "Le nom de la finition ne peut pas dépasser 25 caractères.")]
        public string? CarTrimName { get; set; }
    }
}
