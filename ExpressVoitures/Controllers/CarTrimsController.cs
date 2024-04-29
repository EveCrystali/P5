using ExpressVoitures.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    [Authorize]
    public class CarTrimsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: CarTrims/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            return View();
        }

        // POST: CarTrims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarModelId,CarTrimName")] CarTrim carTrim)
        {
            if (!ModelState.IsValid)
            {
                foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            else if (ModelState.IsValid)
            {
                if (_context.CarTrim.Any(ct => ct.CarTrimName == carTrim.CarTrimName))
                {
                   ModelState.AddModelError("CarTrimName", "La finition existe déjà !");
                }
                else
                {
                    _context.Add(carTrim);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Create", "Cars");
                }
            }

            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carTrim.CarModelId);

            return View(carTrim);
        }

        // GET: CarTrims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarTrim? carTrim = await _context.CarTrim
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);

            return carTrim == null ? NotFound() : View(carTrim);
        }

        // POST: CarTrims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CarTrim? carTrim = await _context.CarTrim.FindAsync(id);
            if (carTrim != null)
            {
                _context.CarTrim.Remove(carTrim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CarTrims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarTrim? carTrim = await _context.CarTrim
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);

            return carTrim == null ? NotFound() : View(carTrim);
        }

        // GET: CarTrims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarTrim? carTrim = await _context.CarTrim.FindAsync(id);
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carTrim?.CarModelId);

            return carTrim == null ? NotFound() : View(carTrim);
        }

        // POST: CarTrims/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarModelId,TrimName")] CarTrim carTrim)
        {
            if (id != carTrim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carTrim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!CarTrimExists(carTrim.Id))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carTrim.CarModelId);

            return View(carTrim);
        }

        // GET: CarTrims
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarTrim.Include(c => c.CarModel).ToListAsync());
        }

        private bool CarTrimExists(int id)
        {
            return _context.CarTrim.Any(e => e.Id == id);
        }
    }
}
