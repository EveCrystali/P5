using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories;

namespace ExpressVoitures.Models.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public List<CarViewModel> GetAllCarsViewModel()
        {
            IEnumerable<Car> carEntities = GetAllCars();
            return MapToCarViewModel(carEntities);
        }

        private List<CarViewModel> MapToCarViewModel(IEnumerable<Car> carEntities)
        {
            List<CarViewModel> carsViewModel = new();
            foreach (Car car in carEntities)
            {
                carsViewModel.Add(new CarViewModel
                {
                    Id = car.Id,
                    CarBrand = car.CarBrand,

                    CarBrandName = car.CarModel?.ModelName ?? "Marque inconnu",

                    CarModel = car.CarModel,
                    
                    CarModelName = car.CarModel?.ModelName ?? "Modèle inconnu",

                    CarTrim = car.CarTrim,
                    CarRepairs = car.CarRepairs,
                    Year = car.Year,
                    Mileage = car.Mileage,
                    PurchasePrice = car.PurchasePrice,
                    SellingPrice = car.SellingPrice,
                    IsAvailable = car.IsAvailable,
                    PurchaseDate = car.PurchaseDate,
                    DateOfAvailability = car.DateOfAvailability,
                    SaleDate = car.SaleDate,
                    Description = car.Description,
                    ImagePaths = car.ImagePaths
                });
            }

            return carsViewModel;
        }

        public List<Car> GetAllCars()
        {
            IEnumerable<Car> carEntities = _carRepository.GetAllCars();
            return carEntities.ToList();
        }

        public CarViewModel GetCarViewModelById(int id)
        {
            List<CarViewModel> carViewModels = GetAllCarsViewModel().ToList();
            return carViewModels.Find(c => c.Id == id);
        }

        public Car GetCarById(int id)
        {
            List<Car> cars = GetAllCars().ToList();
            return cars.Find(c => c.Id == id);
        }

        public async Task<Car> GetCar(int id)
        {
            var cars = await _carRepository.GetCarById(id);
            return cars;
        }

        public async Task<IList<Car>> GetCar()
        {
            var cars = await _carRepository.GetCar();
            return cars;
        }

        private static Car MapToCarEntity(CarViewModel carViewModel)
        {
            Car carEntity = new()
            {
                Id = carViewModel.Id,
                CarBrand = carViewModel.CarBrand,
                CarModel = carViewModel.CarModel,
                CarTrim = carViewModel.CarTrim,
                CarRepairs = carViewModel.CarRepairs,
                Year = carViewModel.Year,
                Mileage = carViewModel.Mileage,
                PurchasePrice = carViewModel.PurchasePrice,
                SellingPrice = carViewModel.SellingPrice,
                IsAvailable = carViewModel.IsAvailable,
                PurchaseDate = carViewModel.PurchaseDate,
                DateOfAvailability = carViewModel.DateOfAvailability,
                SaleDate = carViewModel.SaleDate,
                Description = carViewModel.Description,
                ImagePaths = carViewModel.ImagePaths
            };
            return carEntity;
        }

        public void SaveCar(CarViewModel car)
        {
            var carToAdd = MapToCarEntity(car);
            _carRepository.SaveCar(carToAdd);
        }

        public void DeleteCar(int id)
        {
            _carRepository.DeleteCar(id);
        }
    }
}