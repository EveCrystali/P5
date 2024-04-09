
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

        public CarsController(ApplicationDbContext context, ILogger<CarsController> Logger, ICarService carService)
        {
            _context = context;
            _logger = Logger;
            _carService = carService;
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
            var car = _context.Car
                .Include(c => c.CarBrand)
                .Include(c => c.CarModel)
                .Include(c => c.CarTrim);

            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName");
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,CarBrandId, CarBrandName,CarModelId,CarTrimId,Year,Mileage,PurchasePrice,SellingPrice,IsAvailable,PurchaseDate,DateOfAvailability,SaleDate,Description,ImagePaths, Images")] Car car)
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {
            // _logger.LogInformation("Create is called");
            //ModelState.Remove("Images");
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is NOT valid");
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        // Log or print the error
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                // ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "Brand", carViewModel.CarBrand.Id);
                // ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "ModelName",  carViewModel.CarModel.Id);
                // ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "TrimName", carViewModel.CarTrim.Id);
                ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName", carViewModel.CarBrand.Id);
                ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carViewModel.CarModel.Id);
                ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "CarTrimName", carViewModel.CarTrim.Id);

                return View(carViewModel);
            }
            else
            {
                _logger.LogInformation("ModelState is valid");

                _carService.SaveCar(carViewModel);


                // _context.Add(car);
                // await _context.SaveChangesAsync();

                // var i = 0;

                // foreach (var image in car.Images)
                // {

                //     if (image != null && image.Length > 0)
                //     {
                //         _logger.LogInformation("image != null");
                //         var fileExtension = Path.GetExtension(image.FileName);
                //         var fileName = Path.GetFileName(image.FileName);

                //         var folderDestinationName = car.Id.ToString() + "_" + car.CarModel.ModelName + "_" + car.CarTrim.TrimName;
                //         var fileNewName = "_NoImg" + i.ToString() + fileExtension; // New file name
                //         var folderDestinationPath = Path.Combine(Directory.GetCurrentDirectory(), folderDestinationName);

                //         if (!Directory.Exists(folderDestinationPath))
                //         {
                //             Directory.CreateDirectory(folderDestinationPath);
                //         }

                //         var fullNewFilePath = Path.Combine(folderDestinationPath, "wwwroot/images", fileNewName); // Full path for the new file

                //         _logger.LogInformation("fileName = " + fileName);
                //         _logger.LogInformation("folderDestinationPath = " + folderDestinationPath);
                //         _logger.LogInformation("fullNewFilePath = " + fullNewFilePath);

                //         using (var stream = new FileStream(fullNewFilePath, FileMode.Create))
                //         {
                //             await image.CopyToAsync(stream);
                //             i++;
                //         }

                //         try
                //         {
                //             if (car.ImagePaths == null)
                //             {
                //                 car.ImagePaths = new List<string>();
                //             }
                //             if (fullNewFilePath != null)
                //             {
                //                 car.ImagePaths.Add(fullNewFilePath); // Store the full path of the new file
                //             }
                //         }
                //         catch (Exception ex)
                //         {
                //             _logger.LogError(ex, "Error while adding the image path to the list.");
                //         }
                //     }
                // }   
                // await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "Brand", car.CarBrandId);
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "ModelName", car.CarModelId);
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "TrimName", car.CarTrimId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarBrandId,CarModelId,CarTrimId,Year,Mileage,PurchasePrice,SellingPrice,IsAvailable,PurchaseDate,DateOfAvailability,SaleDate,Description,ImagePaths")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var carBrands = _context.CarBrand ?? Enumerable.Empty<CarBrand>();
            var carModels = _context.CarModel ?? Enumerable.Empty<CarModel>();
            var carTrims = _context.CarTrim ?? Enumerable.Empty<CarTrim>();

            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "CarBrandName", car.CarBrandId);
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", car.CarModelId);
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "TrimName", car.CarTrimId);
            return View(car);
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