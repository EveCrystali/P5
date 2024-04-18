using System.Diagnostics;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICarService _carService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ICarService carService)
        {
            _logger = logger;
            _context = context;
            _carService = carService;
        }

        public IActionResult Index()
        {
            _context.Car
                .Include(c => c.CarModel) // Eager loading CarModel
                .Include(c => c.CarBrand); // Eager loading CarBrand
            IEnumerable<CarViewModel> carViewModels = _carService.GetAllCarsViewModel();

            return View(carViewModels);
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
                {
                    _context.Car
                    .Include(c => c.CarModel) // Eager loading CarModel
                    .Include(c => c.CarBrand); // Eager loading CarBrand
                    IEnumerable<CarViewModel> carViewModelsNoSearch = _carService.GetAllCarsViewModel();
                    return PartialView("_CarCards", carViewModelsNoSearch);
                }

                List<CarViewModel> carViewModels = _carService.GetAllCarsViewModel();
                List<CarViewModel> filteredCars = carViewModels.Where(c => c.CarModel.CarModelName.Contains(query))
                                                                .ToList();
                return PartialView("_CarCards", filteredCars);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred during search: {Error}", ex.Message);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
