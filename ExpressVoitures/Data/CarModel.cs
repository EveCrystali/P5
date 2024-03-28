﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressVoitures.Data;


namespace ExpressVoitures.Data
{
    public class CarModel
    {
        public int Id { get; set; }

        [DisplayName("Modèle")]
        [Required(ErrorMessage = "Le nom du modèle est obligatoire.")]
        public string Model { get; set; }


        [DisplayName("Finition")]
        public string? Trim { get; set; }

    }
}
