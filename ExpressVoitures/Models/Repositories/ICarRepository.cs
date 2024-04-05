using ExpressVoitures.Models.Entities;
using ExpressVoitures.Data;

namespace ExpressVoitures.Models.Repositories
{
    public interface ICarRepository
    {
        Task<Car> GetCarById(int id);

        Task<IList<Car>> GetCar();

        IEnumerable<Car> GetAllCars();

        void SaveCar(Car car);

        void DeleteCar(int id);
    }
}