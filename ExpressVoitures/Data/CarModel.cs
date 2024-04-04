﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressVoitures.Data;


namespace ExpressVoitures.Data
{
    public class CarModel
    {
        public int Id { get; set; }

        public int CarBrandId { get; set; }
        public virtual CarBrand? CarBrand { get; set; }


        [DisplayName("Modèle")]
        [Required(ErrorMessage = "Le nom du modèle est obligatoire.")]
        public string? ModelName { get; set; }

        public virtual ICollection<CarTrim>? Trims { get; set; }

    }
}
