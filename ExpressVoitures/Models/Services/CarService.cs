using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public List<CarViewModel> MapToCarViewModel(IEnumerable<Car> carEntities)
        {
            List<CarViewModel> carsViewModel = new();
            foreach (Car car in carEntities)
            {
                carsViewModel.Add(new CarViewModel
                {
                    Id = car.Id,
                    CarBrandId = car.CarBrandId,
                    CarBrand = car.CarBrand,
                    CarBrandName = car.CarModel?.CarModelName ?? "Marque inconnu",
                    CarModelId = car.CarModelId,
                    CarModel = car.CarModel,
                    CarModelName = car.CarModel?.CarModelName ?? "Modèle inconnu",
                    CarTrimId = car.CarTrimId,
                    CarTrim = car.CarTrim,
                    CarTrimName = car.CarTrim?.CarTrimName ?? "Version inconnue",
                    CarMotorId = car.CarMotorId,
                    CarMotor = car.CarMotor,
                    CarMotorName = car.CarMotor?.CarMotorName ?? "Moteur inconnu",
                    CarRepairs = car.CarRepairs ?? new List<CarRepair>(),
                    Year = car.Year,
                    Mileage = car.Mileage,
                    PurchasePrice = car.PurchasePrice,
                    SellingPrice = car.SellingPrice,
                    IsAvailable = car.IsAvailable,
                    PurchaseDate = car.PurchaseDate,
                    DateOfAvailability = car.DateOfAvailability,
                    SaleDate = car.SaleDate,
                    Description = car.Description,
                    ImagePaths = car.ImagePaths,
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

        public async Task<Car> GetCarById(int id)
        {
            List<Car> cars = GetAllCars().ToList();
            return cars.Find(c => c.Id == id);
        }

        public List<CarBrand> GetAllCarBrands()
        {
            IEnumerable<CarBrand> carBrands = _carRepository.GetAllCarBrands();
            return carBrands.ToList();
        }

        public CarBrand GetCarBrandById(int id)
        {
            List<CarBrand> carBrands = GetAllCarBrands().ToList();
            return carBrands.Find(c => c.Id == id);
        }

        public async Task<Car> GetCar(int id)
        {
            var cars = await GetCarById(id);
            return cars;
        }

        public async Task<IList<Car>> GetCar()
        {
            var cars = await _carRepository.GetCar();
            return cars;
        }

        private async Task<bool> CarExistAsync(int id)
        {
            var existingCar = await GetCar(id);
            return existingCar != null;
        }

        public async Task<Car> MapToCarEntityAsync(CarViewModel carViewModel)
        {
            if (await CarExistAsync(carViewModel.Id))
            {
                var existingCar = await GetCar(carViewModel.Id);
                existingCar.CarBrandId = carViewModel.CarBrandId;
                existingCar.CarModelId = carViewModel.CarModelId;
                existingCar.CarTrimId = carViewModel.CarTrimId;
                existingCar.CarMotorId = carViewModel.CarMotorId;

                existingCar.CarRepairs = new List<CarRepair>();
                foreach (var repairViewModel in carViewModel.CarRepairs)
                {
                    var repair = existingCar.CarRepairs.FirstOrDefault(r => r.Id == repairViewModel.Id);
                    if (repair != null)
                    {
                        // Update existing repair details
                        repair.RepairCost = repairViewModel.RepairCost;
                        repair.RepairDescription = repairViewModel.RepairDescription;
                    }
                    else
                    {
                        // Add as new repair if not found
                        existingCar.CarRepairs.Add(new CarRepair
                        {
                            CarId = existingCar.Id,
                            RepairCost = repairViewModel.RepairCost,
                            RepairDescription = repairViewModel.RepairDescription
                        });
                    }
                }
                existingCar.CarRepairs = carViewModel.CarRepairs;
                existingCar.Year = carViewModel.Year;
                existingCar.Mileage = carViewModel.Mileage;
                existingCar.PurchasePrice = carViewModel.PurchasePrice;
                existingCar.SellingPrice = carViewModel.SellingPrice;
                existingCar.IsAvailable = carViewModel.IsAvailable;
                existingCar.PurchaseDate = carViewModel.PurchaseDate;
                existingCar.DateOfAvailability = carViewModel.DateOfAvailability;
                existingCar.SaleDate = carViewModel.SaleDate;
                existingCar.Description = carViewModel.Description;
                //existingCar.ImagePaths = carViewModel.ImagePaths;

                return existingCar;
            }
            else
            {
                Car carEntity = new()
                {
                    CarBrandId = carViewModel.CarBrandId,
                    CarModelId = carViewModel.CarModelId,
                    CarTrimId = carViewModel.CarTrimId,
                    CarMotorId = carViewModel.CarMotorId,
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
        }

        public async Task SaveCar(CarViewModel car)
        {
            var carToAdd = await MapToCarEntityAsync(car);
            _carRepository.SaveCar(carToAdd);
        }

        public void DeleteCar(int id)
        {
            _carRepository.DeleteCar(id);
        }
    }
}