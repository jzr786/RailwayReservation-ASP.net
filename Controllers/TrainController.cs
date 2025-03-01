using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Data;
using RailwayReservation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{
    [Authorize(Roles = "Admin")] // Ensures only authenticated users can access train management
    public class TrainController : Controller
    {
        private readonly RailwayContext _context;

        public TrainController(RailwayContext context)
        {
            _context = context;
        }

        // GET: Train
        public async Task<IActionResult> Index()
        {
            var trains = await _context.Trains.ToListAsync();
            return View(trains);
        }

        // GET: Train/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Train/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Train train)
        {
            if (ModelState.IsValid)
            {
                _context.Add(train);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(train);
        }

        // GET: Train/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var train = await _context.Trains.FirstOrDefaultAsync(m => m.TrainNo == id);
            if (train == null) return NotFound();

            return View(train);
        }

        // GET: Train/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var train = await _context.Trains.FindAsync(id);
            if (train == null) return NotFound();

            return View(train);
        }

        // POST: Train/Edit/5
        [HttpPost]
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

        // GET: Train/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var train = await _context.Trains.FirstOrDefaultAsync(m => m.TrainNo == id);
            if (train == null) return NotFound();

            return View(train);
        }

        // POST: Train/Delete/5
        [HttpPost, ActionName("Delete")]
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

