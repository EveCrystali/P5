using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressVoitures.Data;

namespace ExpressVoitures.Data
{
    public class CarBrand
    {
        public int Id { get; set; }


        [DisplayName("Marque")]
        [Required(ErrorMessage = "La marque est obligatoire.")]
        public string Brand { get; set; }

    }
}