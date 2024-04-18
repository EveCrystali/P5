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
    public class CarRepairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarRepairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarRepairs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarRepair.Include(c => c.Car);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarRepairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carRepair = await _context.CarRepair
                .Include(c => c.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carRepair == null)
            {
                return NotFound();
            }

            return View(carRepair);
        }

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

        // GET: CarRepairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carRepair = await _context.CarRepair.FindAsync(id);
            if (carRepair == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", carRepair.CarId);
            return View(carRepair);
        }

        // POST: CarRepairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarRepairExists(carRepair.Id))
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

            var carRepair = await _context.CarRepair
                .Include(c => c.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carRepair == null)
            {
                return NotFound();
            }

            return View(carRepair);
        }

        // POST: CarRepairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carRepair = await _context.CarRepair.FindAsync(id);
            if (carRepair != null)
            {
                _context.CarRepair.Remove(carRepair);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarRepairExists(int id)
        {
            return _context.CarRepair.Any(e => e.Id == id);
        }
    }
}
