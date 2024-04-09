using ExpressVoitures.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Models.Repositories
{
    public class CarRepository : ICarRepository
    {
        private static ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Car> GetCarById(int id)
        {
            Car? car = await _context.Car.SingleOrDefaultAsync(c => c.Id == id);
            if (car != null)
            {
                return car;
            }
            else
            {
                // Handle the case when the car is not found
                throw new Exception("Car not found");
            }
        }

        public IEnumerable<CarBrand> GetAllCarBrands()
        {
            return _context.CarBrand.ToList();
        }

        public async Task<CarBrand> GetCarBrandById(int id)
        {
            Car? car = await _context.Car.SingleOrDefaultAsync(c => c.Id == id);
            if (car != null)
            {
                return car.CarBrand;
            }
            else
            {
                // Handle the case when the car is not found
                throw new Exception("CarBrand not found");
            }
        }


        public IEnumerable<CarModel> GetAllCarModels()
        {
            return _context.CarModel.ToList();
        }

        public async Task<CarModel> GetCarModelById(int id)
        {
            Car? car = await _context.Car.SingleOrDefaultAsync(c => c.Id == id);
            if (car != null)
            {
                return car.CarModel;
            }
            else
            {
                // Handle the case when the car is not found
                throw new Exception("CarModel not found");
            }
        }


        public async Task<IList<Car>> GetCar()
        {
            var cars = await _context.Car.ToListAsync();
            return cars;
        }

        public IEnumerable<Car> GetAllCars()
        {
            IEnumerable<Car> carEntities = _context.Car
                                                   .Include(c => c.CarBrand) // Eager loading for CarBrand
                                                   .Include(c => c.CarModel) // Eager loading for CarModel
                                                   .Include(c => c.CarTrim); // Eager loading for CarModel
            return carEntities.ToList();
        }

        public void SaveCar(Car car)
        {
            _context.Car.Add(car);
            _context.SaveChanges();
        }

        public void DeleteCar(int id)
        {
            Car car = _context.Car.FirstOrDefault(c => c.Id == id);
            if (car != null)
            {
                _context.Car.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}