using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories;

namespace ExpressVoitures.Models.Services
{
    public class CarService(ICarRepository carRepository) : ICarService
    {
        private readonly ICarRepository _carRepository = carRepository;

        public void DeleteCar(int id)
        {
            _carRepository.DeleteCar(id);
        }

        public List<CarBrand> GetAllCarBrands()
        {
            return _carRepository.GetAllCarBrands().ToList();
        }

        public List<Car> GetAllCars()
        {
            return _carRepository.GetAllCars().ToList();
        }

        public List<CarViewModel> GetAllCarsViewModel()
        {
            return MapToCarViewModel(GetAllCars());
        }

        public async Task<Car> GetCar(int id)
        {
            return await GetCarById(id);
        }

        public async Task<IList<Car>> GetCar()
        {
            return await _carRepository.GetCar();
        }

        public CarBrand GetCarBrandById(int id)
        {
            return GetAllCarBrands().ToList().Find(c => c.Id == id);
        }

        public async Task<Car> GetCarById(int id)
        {
            return GetAllCars().ToList().Find(c => c.Id == id);
        }

        public CarViewModel GetCarViewModelById(int id)
        {
            return GetAllCarsViewModel().ToList().Find(c => c.Id == id);
        }

        public async Task<Car> MapToCarEntityAsync(CarViewModel carViewModel)
        {
            if (await CarExistAsync(carViewModel.Id))
            {
                Car existingCar = await GetCar(carViewModel.Id);
                existingCar.CodeVIN = carViewModel.CodeVIN;
                existingCar.CarBrandId = carViewModel.CarBrandId;
                existingCar.CarModelId = carViewModel.CarModelId;
                existingCar.CarTrimId = carViewModel.CarTrimId;
                existingCar.CarMotorId = carViewModel.CarMotorId;
                existingCar.CarRepairs = [];

                foreach (CarRepair repairViewModel in carViewModel.CarRepairs)
                {
                    CarRepair? repair = existingCar.CarRepairs.Find(r => r.Id == repairViewModel.Id);
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

                return existingCar;
            }
            else
            {
                Car carEntity = new()
                {
                    CodeVIN = carViewModel.CodeVIN,
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

        public List<CarViewModel> MapToCarViewModel(IEnumerable<Car> carEntities)
        {
            List<CarViewModel> carsViewModel = [];
            foreach (Car car in carEntities)
            {
                carsViewModel.Add(new CarViewModel
                {
                    Id = car.Id,
                    CodeVIN = car.CodeVIN,
                    CarBrandId = car.CarBrandId,
                    CarBrand = car.CarBrand,
                    CarBrandName = car.CarBrand?.CarBrandName ?? "Marque inconnu",
                    CarModelId = car.CarModelId,
                    CarModel = car.CarModel,
                    CarModelName = car.CarModel?.CarModelName ?? "Modèle inconnu",
                    CarTrimId = car.CarTrimId,
                    CarTrim = car.CarTrim,
                    CarTrimName = car.CarTrim?.CarTrimName ?? "Version inconnue",
                    CarMotorId = car.CarMotorId,
                    CarMotor = car.CarMotor,
                    CarMotorName = car.CarMotor?.CarMotorName ?? "Moteur inconnu",
                    CarRepairs = car.CarRepairs ?? [],
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

        public async Task SaveCar(CarViewModel car)
        {
            Car carToAdd = await MapToCarEntityAsync(car);
            _carRepository.SaveCar(carToAdd);
        }

        private async Task<bool> CarExistAsync(int id)
        {
            Car existingCar = await GetCar(id);
            return existingCar != null;
        }
    }
}
