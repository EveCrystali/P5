﻿
using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpressVoitures.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsController> _logger;

        private readonly ICarService _carService;

        private readonly ICarRepository _carRepository;

        public CarsController(ApplicationDbContext context, ILogger<CarsController> Logger, ICarService carService, ICarRepository carRepository)
        {
            _context = context;
            _logger = Logger;
            _carService = carService;
            _carRepository = carRepository;
        }


        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Car.Include(c => c.CarBrand).Include(c => c.CarModel).Include(c => c.CarTrim);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.CarBrand)
                .Include(c => c.CarModel)
                .Include(c => c.CarTrim)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName");
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName");
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is NOT valid");
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName", carViewModel.CarBrand.Id);
                ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carViewModel.CarModel.Id);
                ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName", carViewModel.CarTrim.Id);
                return View(carViewModel);
            }
            else
            {
                _logger.LogInformation("ModelState is valid");

                Car car = _carService.MapToCarEntity(carViewModel);
                _context.Car.Add(car);
                _context.SaveChanges();
                carViewModel.Id = car.Id;
                var newImagePaths = await UploadCarImages(carViewModel);
                car.ImagePaths = newImagePaths;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

        }

        private async Task<List<string>> UploadCarImages(CarViewModel carViewModel)
        {
            var carFromDb = await _context.Car.FindAsync(carViewModel.Id);

            if (carFromDb == null)
            {
                carFromDb = new Car();
                _context.Car.Add(carFromDb);
                return new List<string>();
            }

            if (carFromDb.ImagePaths == null)
            {
                carFromDb.ImagePaths = new List<string>();
            }

            var carBrandName = _context.CarBrand.FirstOrDefault(b => b.Id == carViewModel.CarBrandId)?.CarBrandName;
            var carModelName = _context.CarModel.FirstOrDefault(m => m.Id == carViewModel.CarModelId)?.CarModelName;
            var carTrimName = _context.CarTrim.FirstOrDefault(m => m.Id == carViewModel.CarTrimId)?.CarTrimName;

            var imagePaths = new List<string>();
            var i = 1;
            if (carViewModel.Images != null)
            {
                foreach (var image in carViewModel.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileExtension = Path.GetExtension(image.FileName);
                        var fileName = $"Img{i}{fileExtension}";
                        var folderName = $"{carViewModel.Id}_{carBrandName}_{carModelName}_{carTrimName}";
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var fullPath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        imagePaths.Add(fullPath);
                        i++;
                    }
                }
            }
            carFromDb.ImagePaths.AddRange(imagePaths);

            await _context.SaveChangesAsync();

            return imagePaths;
        }


        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            var carViewModel = _carService.GetCarViewModelById(car.Id);

            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName");
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName");

            return View(carViewModel);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarViewModel carViewModel)
        {
            if (id != carViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid");

                var carFromDb = await _context.Car.FindAsync(id);
                if (carFromDb == null)
                {
                    return NotFound();
                }

                await UploadCarImages(carViewModel);
                _context.Update(carFromDb);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Index));

            }

            var carBrands = _context.CarBrand ?? Enumerable.Empty<CarBrand>();
            var carModels = _context.CarModel ?? Enumerable.Empty<CarModel>();
            var carTrims = _context.CarTrim ?? Enumerable.Empty<CarTrim>();

            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName", carViewModel.CarBrandId);
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carViewModel.CarModelId);
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimeName", carViewModel.CarTrimId);
            return View(carViewModel);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.CarBrand)
                .Include(c => c.CarModel)
                .Include(c => c.CarTrim)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                _context.Car.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.Id == id);
        }

        public IActionResult GetModelsByBrand(int brandId)
        {
            _logger.LogInformation("GetModelsByBrand : Recherche des Models pour le modèle avec l'ID : " + brandId);
            var models = _context.CarModel
                .Where(m => m.CarBrandId == brandId)
                .Select(m => new { m.Id, m.CarModelName })
                .ToList();
            if (!models.Any())
            {
                _logger.LogInformation("Aucun modeles trouvée pour le modele avec l'ID : " + brandId);
            }
            else
            {
                foreach (var model in models)
                {
                    _logger.LogInformation("Modeles trouvés Id : " + model.Id + " Nom : " + model.CarModelName);
                }
            }
            return Json(models);
        }

        public IActionResult GetTrimsByModel(int modelId)
        {
            _logger.LogInformation("GetTrimsByModel : Recherche des finitions pour le modèle avec l'Id : " + modelId);
            var trims = _context.CarTrim
                .Where(t => t.CarModelId == modelId)
                .Select(t => new { t.Id, t.CarTrimName })
                .ToList();

            if (!trims.Any())
            {
                _logger.LogInformation("Aucune finition trouvée pour le modèle avec l'Id : " + modelId);
            }
            else
            {
                foreach (var trim in trims)
                {
                    _logger.LogInformation("Finitions trouvés Id : " + trim.Id + " Nom : " + trim.CarTrimName);
                }
            }

            return Json(trims);
        }
    }
}