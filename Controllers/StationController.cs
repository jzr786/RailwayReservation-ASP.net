using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Data;
using RailwayReservation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{

    public class StationController : Controller
    {
        private readonly RailwayContext _context;

        public StationController(RailwayContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var stations = _context.Stations.ToList();
            return View(stations);
        }

        
        public IActionResult Details(int id)
        {
            var station = _context.Stations.FirstOrDefault(s => s.Id == id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Station station)
        {
            if (ModelState.IsValid)
            {
                _context.Stations.Add(station);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Station/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var station = _context.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // POST: Station/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Station station)
        {
            if (id != station.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Stations.Update(station);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Station/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var station = _context.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // POST: Station/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }

            try
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Station {station.Name} deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error deleting station: {ex.Message}");
                ModelState.AddModelError("", "This station cannot be deleted as it is referenced elsewhere.");
                return View(station);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}