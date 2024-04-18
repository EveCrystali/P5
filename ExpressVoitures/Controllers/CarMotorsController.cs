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
    public class CarMotorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarMotorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarMotors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarMotor.Include(c => c.CarModel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarMotors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carMotor = await _context.CarMotor
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carMotor == null)
            {
                return NotFound();
            }

            return View(carMotor);
        }

        // GET: CarMotors/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName");
            return View();
        }

        // POST: CarMotors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: CarMotors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carMotor = await _context.CarMotor.FindAsync(id);
            if (carMotor == null)
            {
                return NotFound();
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModel, "Id", "CarModelName", carMotor.CarModelId);
            return View(carMotor);
        }

        // POST: CarMotors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarMotorExists(carMotor.Id))
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

            var carMotor = await _context.CarMotor
                .Include(c => c.CarModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carMotor == null)
            {
                return NotFound();
            }

            return View(carMotor);
        }

        // POST: CarMotors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carMotor = await _context.CarMotor.FindAsync(id);
            if (carMotor != null)
            {
                _context.CarMotor.Remove(carMotor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarMotorExists(int id)
        {
            return _context.CarMotor.Any(e => e.Id == id);
        }
    }
}
