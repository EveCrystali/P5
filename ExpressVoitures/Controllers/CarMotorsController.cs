using ExpressVoitures.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    [Authorize]
    public class CarMotorsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: CarMotors/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            return View();
        }

        // POST: CarMotors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarModelId,CarMotorName")] CarMotor carMotor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carMotor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carMotor.CarModelId);
            return View(carMotor);
        }

        // GET: CarMotors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarMotor? carMotor = await _context.CarMotor
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);

            return carMotor == null ? NotFound() : View(carMotor);
        }

        // POST: CarMotors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CarMotor? carMotor = await _context.CarMotor.FindAsync(id);
            if (carMotor != null)
            {
                _context.CarMotor.Remove(carMotor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CarMotors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarMotor? carMotor = await _context.CarMotor
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);

            return carMotor == null ? NotFound() : View(carMotor);
        }

        // GET: CarMotors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarMotor? carMotor = await _context.CarMotor.FindAsync(id);
            if (carMotor == null)
            {
                return NotFound();
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carMotor.CarModelId);
            return View(carMotor);
        }

        // POST: CarMotors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarModelId,CarMotorName")] CarMotor carMotor)
        {
            if (id != carMotor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carMotor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!CarMotorExists(carMotor.Id))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carMotor.CarModelId);
            return View(carMotor);
        }

        // GET: CarMotors
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarMotor.Include(c => c.CarModel).ToListAsync());
        }

        private bool CarMotorExists(int id)
        {
            return _context.CarMotor.Any(e => e.Id == id);
        }
    }
}
