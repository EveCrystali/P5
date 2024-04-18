using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpressVoitures.Data;
using Microsoft.AspNetCore.Authorization;

namespace ExpressVoitures.Controllers
{
    [Authorize]
    public class CarTrimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarTrimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarTrims
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarTrim.Include(c => c.CarModel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarTrims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carTrim = await _context.CarTrim
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carTrim == null)
            {
                return NotFound();
            }

            return View(carTrim);
        }

        // GET: CarTrims/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            return View();
        }

        // POST: CarTrims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarModelId,CarTrimName")] CarTrim carTrim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carTrim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carTrim.CarModelId);
            return View(carTrim);
        }

        // GET: CarTrims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carTrim = await _context.CarTrim.FindAsync(id);
            if (carTrim == null)
            {
                return NotFound();
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carTrim.CarModelId);
            return View(carTrim);
        }

        // POST: CarTrims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarTrimExists(carTrim.Id))
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

            var carTrim = await _context.CarTrim
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carTrim == null)
            {
                return NotFound();
            }

            return View(carTrim);
        }

        // POST: CarTrims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carTrim = await _context.CarTrim.FindAsync(id);
            if (carTrim != null)
            {
                _context.CarTrim.Remove(carTrim);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarTrimExists(int id)
        {
            return _context.CarTrim.Any(e => e.Id == id);
        }
    }
}
