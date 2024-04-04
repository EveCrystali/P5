using ExpressVoitures.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsController> _logger;

        public CarsController(ApplicationDbContext context, ILogger<CarsController> Logger)
        {
            _context = context;
            _logger = Logger;
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
            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "Brand");
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "ModelName");
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "TrimName");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarBrandId,CarModelId,CarTrimId,Year,Mileage,PurchasePrice,SellingPrice,IsAvailable,PurchaseDate,DateOfAvailability,SaleDate,Description,ImagePaths")] Car car)
        {
            _logger.LogInformation("Create is called");
            ModelState.Remove("Images");
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
            }
            else if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid");
                foreach (var image in car.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        _logger.LogInformation("image != null");

                       var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        _logger.LogInformation("fileName = " + fileName);
                        _logger.LogInformation("filePath = " + filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        try
                        {
                            if (car.ImagePaths == null)
                            {
                                car.ImagePaths = new List<string>();
                            }
                            if (filePath != null)
                            {
                                car.ImagePaths.Add(filePath);

                            }
                        }
                        catch (Exception ex)
                        {
                            // Log l'erreur ou effectuez une autre action
                            _logger.LogError(ex, "Erreur lors de l'ajout du chemin de l'image à la liste.");
                        }
                    }
                }

                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "Brand", car.CarBrandId);
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "ModelName", car.CarModelId);
            ViewData["CarTrimId"] = new SelectList(_context.CarTrim, "Id", "TrimName", car.CarTrimId);
            return View(car);
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

            ViewData["CarBrandId"] = new SelectList(_context.CarBrand, "Id", "Brand", car.CarBrandId);
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "ModelName", car.CarModelId);
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
                .Select(m => new { m.Id, m.ModelName })
                .ToList();
            if (!models.Any())
            {
                _logger.LogInformation("Aucun modeles trouvée pour le modele avec l'ID : " + brandId);
            }
            else
            {
                foreach (var model in models)
                {
                    _logger.LogInformation("Modeles trouvés Id : " + model.Id + " Nom : " + model.ModelName);
                }
            }
            return Json(models);
        }

        public IActionResult GetTrimsByModel(int modelId)
        {
            _logger.LogInformation("GetTrimsByModel : Recherche des finitions pour le modèle avec l'Id : " + modelId);
            var trims = _context.CarTrim
                .Where(t => t.CarModelId == modelId)
                .Select(t => new { t.Id, t.TrimName })
                .ToList();

            if (!trims.Any())
            {
                _logger.LogInformation("Aucune finition trouvée pour le modèle avec l'Id : " + modelId);
            }
            else
            {
                foreach (var trim in trims)
                {
                    _logger.LogInformation("Finitions trouvés Id : " + trim.Id + " Nom : " + trim.TrimName);
                }
            }

            return Json(trims);
        }
    }
}