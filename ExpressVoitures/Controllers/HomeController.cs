using System.Diagnostics;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ICarService carService) : Controller
    {
        private readonly ICarService _carService = carService;
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<HomeController> _logger = logger;
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            _context.Car
                .Include(c => c.CarModel)
                .Include(c => c.CarBrand);
            IEnumerable<CarViewModel> carViewModels = _carService.GetAllCarsViewModel();

            return View(carViewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
                {
                    _context.Car
                    .Include(c => c.CarModel)
                    .Include(c => c.CarBrand);
                    return PartialView("_CarCards", _carService.GetAllCarsViewModel());
                }

                return PartialView("_CarCards", _carService.GetAllCarsViewModel().Where(c => c.CarModel.CarModelName.Contains(query)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred during search: {Error}", ex.Message);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult FilterCarsByAvailability(bool? onlyAvailable)
        {
            try
            {
                List<CarViewModel> cars = _carService.GetAllCarsViewModel();
                bool filterApplied = onlyAvailable.GetValueOrDefault();
                if (filterApplied)
                {
                    cars = cars.Where(c => c.IsAvailable).ToList();
                }

                ViewBag.Trier = filterApplied;
                return PartialView("_CarCards", cars.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred during filtering: {Error}", ex.Message);
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}