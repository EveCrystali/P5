namespace ExpressVoitures.Data
{
    public class CarTrim
    {
        public int Id { get; set; }

        public int CarModelId { get; set; }

        public string? TrimName { get; set; }

        public virtual CarModel? CarModel { get; set; }
    }
}