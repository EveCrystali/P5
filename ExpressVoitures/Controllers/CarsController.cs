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
            var applicationDbContext = _context.Car.Include(c => c.CarBrand).Include(c => c.CarModel).Include(c => c.CarTrim).Include(c => c.CarRepairs);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Car.FirstAsync(c => c.Id == id);
            var carViewModel = _carService.GetCarViewModelById(id);

            return View(carViewModel);
        }

        // GET: Cars/Create
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
                CarRepairs = new List<CarRepair>()
            };

            return View(carViewModel);
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {
            for (int i = 0; i < carViewModel.CarRepairs.Count; i += 1)
            {
                ModelState.Remove($"CarRepairs[{i}].Car");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is NOT valid");
                foreach (var modelState in ViewData.ModelState.Values)
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
                var newImagePaths = await UploadCarImages(carViewModel);
                car.ImagePaths = newImagePaths;

                // Sauvegarde les changements finaux de manière asynchrone
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
            if (carViewModel.Images != null)
            {
                foreach (var image in carViewModel.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileExtension = Path.GetExtension(image.FileName);
                        var fileName = CreateUniqueFileName() + fileExtension;
                        var folderName = $"{carViewModel.Id}_{carBrandName}_{carModelName}_{carTrimName}";
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);
                        var folderRelativePath = Path.Combine("images", folderName);
                        var fileRelativePath = Path.Combine(folderRelativePath, fileName);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var fullPath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
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

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.Include(car => car.CarRepairs).FirstAsync(c => c.Id == id);
            var carViewModel = _carService.GetCarViewModelById(id);

            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName");
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName");
            ViewData["CarMotorId"] = new SelectList(_context.CarMotor, "Id", "CarMotorName");

            return View(carViewModel);
        }

        // POST: Cars/Edit/5
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

                var carFromDb = await _context.Car.FindAsync(id);
                if (carFromDb == null)
                {
                    return NotFound();
                }
                // Delete all existingCar.CarRepair already in database concerning this car WIP
                var carRepairsToDelete = _context.CarRepair.Where(r => r.CarId == carFromDb.Id);
                _context.CarRepair.RemoveRange(carRepairsToDelete);

                await _carService.MapToCarEntityAsync(carViewModel);

                await UploadCarImages(carViewModel);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var carBrands = _context.CarBrand ?? Enumerable.Empty<CarBrand>();
            var carModels = _context.CarModel ?? Enumerable.Empty<CarModel>();
            var carTrims = _context.CarTrim ?? Enumerable.Empty<CarTrim>();
            var carMotors = _context.CarMotor ?? Enumerable.Empty<CarMotor>();

            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName", carViewModel.CarBrandId);
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carViewModel.CarModelId);
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName", carViewModel.CarTrimId);
            ViewData["CarMotor"] = new SelectList(_context.CarMotor, "Id", "CarMotorName", carViewModel.CarMotorId);
            return View(carViewModel);
        }

        public string CreateUniqueFileName()
        {
            var timestamp = DateTimeOffset.Now.ToString("MMddHHmmss");
            var guidPart = Guid.NewGuid().ToString().Substring(0, 8); // Utiliser seulement une partie du Guid pour la brièveté
            return $"Img_{timestamp}_{guidPart}";
        }

        [HttpPost]
        public IActionResult DeleteImage(int carId, string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || carId <= 0)
            {
                return View("Error the parameters are invalid");
            }

            try
            {
                // Assuming imagePath is a relative path to the image file
                var relativePath = imagePath;
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                if (System.IO.File.Exists(fullPath))
                {
                    //Delete the image
                    System.IO.File.Delete(fullPath);

                    //Delete the image path from the database
                    var carFromDb = _context.Car.Find(carId);
                    if (carFromDb != null)
                    {
                        carFromDb.ImagePaths.Remove(relativePath);
                        _context.Update(carFromDb);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return View("Error while trying to delete the image");
            }

            // Redirect back to the edit page or wherever is appropriate after deletion
            return RedirectToAction("Edit", new { id = carId });
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
                .Include(c => c.CarMotor)
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
            var carFromDb = await _context.Car.FindAsync(id);
            if (carFromDb != null)
            {
                var carBrandName = _context.CarBrand.FirstOrDefault(b => b.Id == carFromDb.CarBrandId)?.CarBrandName;
                var carModelName = _context.CarModel.FirstOrDefault(m => m.Id == carFromDb.CarModelId)?.CarModelName;
                var carTrimName = _context.CarTrim.FirstOrDefault(m => m.Id == carFromDb.CarTrimId)?.CarTrimName;

                if (carBrandName != null && carModelName != null && carTrimName != null)
                {
                    var folderName = $"{carFromDb.Id}_{carBrandName}_{carModelName}_{carTrimName}";
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName);

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

        public IActionResult GetMotorsByModel(int modelId)
        {
            _logger.LogInformation("GetMotorsByModel : Searching for motors for the model with Id: " + modelId);
            var motors = _context.CarMotor
                .Where(m => m.CarModelId == modelId)
                .Select(m => new { m.Id, m.CarMotorName })
                .ToList();

            if (!motors.Any())
            {
                _logger.LogInformation("No motors found for the model with Id: " + modelId);
            }
            else
            {
                foreach (var motor in motors)
                {
                    _logger.LogInformation("Motors found Id: " + motor.Id + " Name: " + motor.CarMotorName);
                }
            }

            return Json(motors);
        }
    }
}