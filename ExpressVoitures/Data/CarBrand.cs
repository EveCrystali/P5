using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressVoitures.Data;

namespace ExpressVoitures.Data
{
    public class CarBrand
    {
        public int Id { get; set; }

        public int CarBrandId { get; set; }

        [DisplayName("Marque")]
        [Required(ErrorMessage = "La marque est obligatoire.")]
        public string? CarBrandName { get; set; }

        public virtual ICollection<CarModel>? CarModels { get; set; }

    }
}