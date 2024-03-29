using System;
using System.Diagnostics;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Car.Include(c => c.CarBrand).Include(c => c.CarModel);
            //return View(await applicationDbContext.ToListAsync());

            var cars = await _context.Car.ToListAsync();
            var modelViewCar = cars.Select(x => new ModelViewCar
            {
                IdCar = x.Id,
            }).ToList();
            return View(modelViewCar);
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
