using ExpressVoitures.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Models.Repositories
{
    public class CarRepository : ICarRepository
    {
        private static ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context) => _context = context;

        public void DeleteCar(int id)
        {
            Car car = _context.Car.FirstOrDefault(c => c.Id == id);
            if (car != null)
            {
                _context.Car.Remove(car);
                _context.SaveChanges();
            }
        }

        public IEnumerable<CarBrand> GetAllCarBrands() => [.. _context.CarBrand];

        public IEnumerable<CarModel> GetAllCarModels() => [.. _context.CarModel];

        public IEnumerable<Car> GetAllCars()
        {
            try
            {
                var carEntities = _context.Car
                    .Include(c => c.CarBrand)
                    .Include(c => c.CarModel)
                    .Include(c => c.CarTrim)
                    .Include(c => c.CarMotor)
                    .ToList();

                return carEntities;
            }
            catch (Exception ex)
            {
                // Log l'exception ou traitez-la selon le cas
                return [];
            }
        }

        public async Task<IList<Car>> GetCar() => await _context.Car.ToListAsync();

        public async Task<CarBrand> GetCarBrandById(int id)
        {
            Car? car = await _context.Car.SingleOrDefaultAsync(c => c.Id == id);
            return car != null ? car.CarBrand : throw new KeyNotFoundException("CarBrand not found for provided ID.");
        }

        public async Task<Car> GetCarById(int id)
        {
            Car? car = await _context.Car.SingleOrDefaultAsync(c => c.Id == id);
            return car ?? throw new KeyNotFoundException("Car not found for provided ID.");
        }

        public async Task<CarModel> GetCarModelById(int id)
        {
            Car? car = await _context.Car.SingleOrDefaultAsync(c => c.Id == id);
            return car != null ? car.CarModel : throw new KeyNotFoundException("CarModel not found for provided ID.");
        }

        public void SaveCar(Car car)
        {
            _context.Car.Add(car);
            _context.SaveChanges();
        }
    }
}
