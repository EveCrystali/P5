using ExpressVoitures.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
    [Authorize]
    public class CarRepairsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: CarRepairs/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id");

            return View();
        }

        // POST: CarRepairs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarId,RepairCost,RepairDescription")] CarRepair carRepair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carRepair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", carRepair.CarId);

            return View(carRepair);
        }

        // GET: CarRepairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarRepair? carRepair = await _context.CarRepair
                .Include(c => c.Car)
                .FirstOrDefaultAsync(m => m.Id == id);

            return carRepair == null ? NotFound() : View(carRepair);
        }

        // POST: CarRepairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CarRepair? carRepair = await _context.CarRepair.FindAsync(id);
            if (carRepair != null)
            {
                _context.CarRepair.Remove(carRepair);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: CarRepairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarRepair? carRepair = await _context.CarRepair
                .Include(c => c.Car)
                .FirstOrDefaultAsync(m => m.Id == id);

            return carRepair == null ? NotFound() : View(carRepair);
        }

        // GET: CarRepairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CarRepair? carRepair = await _context.CarRepair.FindAsync(id);

            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", carRepair?.CarId);

            return carRepair == null ? NotFound() : View(carRepair);
        }

        // POST: CarRepairs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,RepairCost,RepairDescription")] CarRepair carRepair)
        {
            if (id != carRepair.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carRepair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!CarRepairExists(carRepair.Id))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", carRepair.CarId);

            return View(carRepair);
        }

        // GET: CarRepairs
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarRepair.Include(c => c.Car).ToListAsync());
        }

        private bool CarRepairExists(int id)
        {
            return _context.CarRepair.Any(e => e.Id == id);
        }
    }
}
