using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Models.Services;

namespace ExpressVoitures.Models.Services
{
    public interface ICarService
    {
        List<Car> GetAllCars();

        List<CarViewModel> GetAllCarsViewModel();

        Car GetCarById(int id);

        CarViewModel GetCarViewModelById(int id);

        Task<Car> GetCar(int id);

        Task<IList<Car>> GetCar();

        void SaveCar(CarViewModel car);

        void DeleteCar(int id);
    }
}