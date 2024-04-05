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

        public async Task<IList<Car>> GetCar()
        {
            var cars = await _context.Car.ToListAsync();
            return cars;
        }

        public IEnumerable<Car> GetAllCars()
        {
            IEnumerable<Car> carEntities = _context.Car;
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