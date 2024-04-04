using System.ComponentModel;

namespace ExpressVoitures.Data
{
    public class CarTrim
    {
        public int Id { get; set; }

        public int CarModelId { get; set; }

        public virtual CarModel? CarModel { get; set; }


        [DisplayName("Finition")]
        public string? TrimName { get; set; }

    }
}