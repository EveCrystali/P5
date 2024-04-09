using System;
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

            var cars = _context.Car
                .Include(c => c.CarModel) // Eager loading CarModel
                .Include(c => c.CarBrand); // Eager loading CarBrand

            IEnumerable<CarViewModel> carViewModels = _carService.GetAllCarsViewModel();

            return View(carViewModels);

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
