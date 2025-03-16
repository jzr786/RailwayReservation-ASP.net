using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Data;
using RailwayReservation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{

    public class TrainController : Controller
    {
        private readonly RailwayContext _context;

        public TrainController(RailwayContext context)
        {
            _context = context;
        }

        // ✅ Visible to all users
        public async Task<IActionResult> Index()
        {
            var trains = await _context.Trains.ToListAsync();
            return View(trains);
        }

        // ✅ Visible to all users
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var train = await _context.Trains.FirstOrDefaultAsync(m => m.TrainNo == id);
            if (train == null) return NotFound();

            return View(train);
        }

        // ✅ Restricted to Admins
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Train train)
        {
            if (ModelState.IsValid)
            {
                _context.Trains.Add(train);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(train);
        }

        // ✅ Restricted to Admins
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var train = await _context.Trains.FindAsync(id);
            if (train == null) return NotFound();

            return View(train);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Train train)
        {
            if (id != train.TrainNo) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(train);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(train);
        }

        // ✅ Restricted to Admins
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var train = await _context.Trains.FirstOrDefaultAsync(m => m.TrainNo == id);
            if (train == null) return NotFound();

            return View(train);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            if (train != null)
            {
                _context.Trains.Remove(train);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

}