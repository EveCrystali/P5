using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Data;

namespace ExpressVoitures.Models.Repositories
{
    public interface ICarRepository
    {
        Task<Car> GetCarById(int id);

        Task<IList<Car>> GetCar();

        IEnumerable<Car> GetAllCars();

        IEnumerable<CarBrand> GetAllCarBrands();

        Task<CarBrand> GetCarBrandById(int id);

        IEnumerable<CarModel> GetAllCarModels();
        
        Task<CarModel> GetCarModelById(int id);

        void SaveCar(Car car);

        void DeleteCar(int id);
    }
}