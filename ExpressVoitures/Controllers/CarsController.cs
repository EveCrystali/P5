using System;
using ExpressVoitures.Data;
using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class CarsController(ApplicationDbContext context, ILogger<CarsController> Logger, ICarService carService) : Controller
    {
        private readonly ICarService _carService = carService;
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<CarsController> _logger = Logger;

        [Authorize]
        public IActionResult Create()
        {
            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName");
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName");
            ViewData["CarMotorId"] = new SelectList(_context.CarMotor, "Id", "CarMotorName");

            var carViewModel = new CarViewModel
            {
                CarBrand = new CarBrand(),
                CarModel = new CarModel(),
                CarTrim = new CarTrim(),
                CarMotor = new CarMotor(),
                CarRepairs = []
            };

            return View(carViewModel);
        }

        [Authorize]
        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {
            for (int i = 0; i < carViewModel.CarRepairs.Count; ++i)
            {
                ModelState.Remove($"CarRepairs[{i}].Car");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is NOT valid");
                foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        _logger.LogInformation(error.ErrorMessage);
                    }
                }

                ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName", carViewModel.CarBrandId);
                ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carViewModel.CarModelId);
                ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName", carViewModel.CarTrimId);
                ViewData["CarMotorId"] = new SelectList(_context.CarMotor, "Id", "CarMotorName", carViewModel.CarMotorId);

                return View(carViewModel);
            }
            else
            {
                _logger.LogInformation("ModelState is valid");

                Car car = await _carService.MapToCarEntityAsync(carViewModel);
                _context.Car.Add(car);
                // Sauvegarde les changements de manière asynchrone pour s'assurer que l'ID de Car est généré
                await _context.SaveChangesAsync();

                carViewModel.Id = car.Id;
                car.ImagePaths = await UploadCarImages(carViewModel);
                // Sauvegarde les changements finaux de manière asynchrone
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public string CreateUniqueFileName(string originalFileName)
        {
            string timestamp = DateTimeOffset.Now.ToString("MMddHHmmss");
            string guidPart = Guid.NewGuid().ToString().Substring(0, 8); 
            return $"Img_{timestamp}_{guidPart}" + Path.GetExtension(originalFileName);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _context.Car
                .Include(c => c.CarBrand)
                .Include(c => c.CarModel)
                .Include(c => c.CarTrim)
                .Include(c => c.CarMotor)
                .FirstOrDefaultAsync(m => m.Id == id);

            return car == null ? NotFound() : View(car);
        }

        // POST: Cars/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Car? carFromDb = await _context.Car.FindAsync(id);
            if (carFromDb != null)
            {
                string? carBrandName = _context.CarBrand.FirstOrDefault(b => b.Id == carFromDb.CarBrandId)?.CarBrandName;
                string? carModelName = _context.CarModel.FirstOrDefault(m => m.Id == carFromDb.CarModelId)?.CarModelName;
                string? carTrimName = _context.CarTrim.FirstOrDefault(m => m.Id == carFromDb.CarTrimId)?.CarTrimName;

                if (carBrandName != null && carModelName != null && carTrimName != null)
                {
                    string folderName = $"{carFromDb.Id}_{carBrandName}_{carModelName}_{carTrimName}";
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);

                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                    }
                }
                _context.Car.Remove(carFromDb);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteImage(int carId, string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || carId <= 0)
            {
                return View("Error", new { errorMessage = "The parameters are invalid" });
            }

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

            try
            {
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound();
                }

                // Delete the image file
                System.IO.File.Delete(fullPath);

                // Delete the image path from the database if it exists
                Car? carFromDb = _context.Car.Find(carId);
                if (carFromDb?.ImagePaths != null && carFromDb.ImagePaths.Contains(imagePath))
                {
                    carFromDb.ImagePaths.Remove(imagePath);
                    _context.Update(carFromDb);
                    _context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while trying to delete the image.");
                return View("Error", new { errorMessage = "An error occurred while trying to delete the image." });
            }

            // Redirect back to the edit page
            return RedirectToAction("Edit", new { id = carId });
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int id)
        {
            await _context.Car.FirstAsync(c => c.Id == id);
            await _context.Car.Include(car => car.CarRepairs).FirstAsync(c => c.Id == id);
            CarViewModel carViewModel = _carService.GetCarViewModelById(id);

            return View(carViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                await _context.Car.Include(car => car.CarRepairs).FirstAsync(c => c.Id == id);
                CarViewModel carViewModel = _carService.GetCarViewModelById((int)id);

                ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName");
                ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
                ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName");
                ViewData["CarMotorId"] = new SelectList(_context.CarMotor.Where(m => m.CarModelId == carViewModel.CarModelId), "Id", "CarMotorName", carViewModel.CarMotorId);

                return View(carViewModel);
            }
        }

        // POST: Cars/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarViewModel carViewModel)
        {
            for (int i = 0; i < carViewModel.CarRepairs.Count; i++)
            {
                ModelState.Remove($"CarRepairs[{i}].Car");
            }

            if (id != carViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid");

                Car? carFromDb = await _context.Car.FindAsync(id);
                if (carFromDb == null)
                {
                    return NotFound();
                }
                // Delete all existingCar.CarRepair already in database concerning this car WIP
                IQueryable<CarRepair> carRepairsToDelete = _context.CarRepair.Where(r => r.CarId == carFromDb.Id);
                _context.CarRepair.RemoveRange(carRepairsToDelete);

                await _carService.MapToCarEntityAsync(carViewModel);

                await UploadCarImages(carViewModel);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            IEnumerable<CarBrand> carBrands = _context.CarBrand ?? Enumerable.Empty<CarBrand>();
            IEnumerable<CarModel> carModels = _context.CarModel ?? Enumerable.Empty<CarModel>();
            IEnumerable<CarTrim> carTrims = _context.CarTrim ?? Enumerable.Empty<CarTrim>();
            IEnumerable<CarMotor> carMotors = _context.CarMotor ?? Enumerable.Empty<CarMotor>();

            ViewData["CarBrandId"] = new SelectList(carBrands, "Id", "CarBrandName", carViewModel.CarBrandId);
            ViewData["CarModelId"] = new SelectList(carModels, "Id", "CarModelName", carViewModel.CarModelId);
            ViewData["CarTrimId"] = new SelectList(carTrims, "Id", "CarTrimName", carViewModel.CarTrimId);
            ViewData["CarMotor"] = new SelectList(carMotors, "Id", "CarMotorName", carViewModel.CarMotorId);

            return View(carViewModel);
        }

        [Authorize]
        public IActionResult GetModelsByBrand(int brandId)
        {
            _logger.LogInformation("GetModelsByBrand : Recherche des Models pour le modèle avec l'ID : " + brandId);
            var models = _context.CarModel
                .Where(m => m.CarBrandId == brandId)
                .Select(m => new { m.Id, m.CarModelName })
                .ToList();
            if (models.Count == 0)
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

        [Authorize]
        public IActionResult GetMotorsByModel(int modelId)
        {
            var motors = _context.CarMotor
                .Where(m => m.CarModelId == modelId)
                .Select(m => new { m.Id, m.CarMotorName })
                .ToList();

            if (motors.Count == 0) 
            {
                _logger.LogInformation("No motors found for the model with Id: " + modelId);
                return Json(new { available = false });
            }

            return Json(motors);
        }

        [Authorize]
        public IActionResult GetTrimsByModel(int modelId)
        {
            _logger.LogInformation("GetTrimsByModel : Recherche des finitions pour le modèle avec l'Id : " + modelId);
            var trims = _context.CarTrim
                .Where(t => t.CarModelId == modelId)
                .Select(t => new { t.Id, t.CarTrimName })
                .ToList();

            if (trims.Count == 0)
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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Car.Include(c => c.CarBrand).Include(c => c.CarModel).Include(c => c.CarTrim).Include(c => c.CarRepairs).ToListAsync());
        }

        [Authorize]
        private async Task<List<string>> UploadCarImages(CarViewModel carViewModel)
        {
            Car? carFromDb = await _context.Car.FindAsync(carViewModel.Id);

            if (carFromDb == null)
            {
                carFromDb = new Car();
                _context.Car.Add(carFromDb);
                return [];
            }

            carFromDb.ImagePaths ??= [];
            List<string> imagePaths = [];
            string? carBrandName = _context.CarBrand.FirstOrDefault(b => b.Id == carViewModel.CarBrandId)?.CarBrandName;
            string? carModelName = _context.CarModel.FirstOrDefault(m => m.Id == carViewModel.CarModelId)?.CarModelName;
            string? carTrimName = _context.CarTrim.FirstOrDefault(m => m.Id == carViewModel.CarTrimId)?.CarTrimName;

            if (carViewModel.Images != null)
            {
                foreach (IFormFile image in carViewModel.Images)
                {
                    if (image?.Length > 0)
                    {
                        string fileName = CreateUniqueFileName(image.FileName);
                        string folderName = $"{carViewModel.Id}_{carBrandName}_{carModelName}_{carTrimName}";
                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);
                        string fileRelativePath = Path.Combine(Path.Combine("images", folderName), fileName);
                        string fullPath = Path.Combine(folderPath, fileName);

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        await using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        imagePaths.Add(fileRelativePath);
                    }
                }
            }

            carFromDb.ImagePaths.AddRange(imagePaths);
            await _context.SaveChangesAsync();

            return imagePaths;
        }
    }
}
