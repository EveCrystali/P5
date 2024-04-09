using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;

namespace ExpressVoitures.Models.Services
{
    public interface ICarService
    {
        List<Car> GetAllCars();

        List<CarViewModel> GetAllCarsViewModel();

        Car GetCarById(int id);

        CarViewModel GetCarViewModelById(int id);

        Car MapToCarEntity(CarViewModel carViewModel);

        List<CarViewModel> MapToCarViewModel(IEnumerable<Car> carEntities);
        Task<Car> GetCar(int id);

        Task<IList<Car>> GetCar();

        void SaveCar(CarViewModel car);

        void DeleteCar(int id);
    }
}