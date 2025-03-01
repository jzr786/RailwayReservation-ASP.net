using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RailwayReservation.Data;
using RailwayReservation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{
    [Authorize(Roles = "Admin")] // Ensures only authenticated users can manage schedules
    public class StationController : Controller
    {
        private readonly RailwayContext _context;

        public StationController(RailwayContext context)
        {
            _context = context;
        }

        // GET: Station
        public IActionResult Index()
        {
            var stations = _context.Stations.ToList();
            return View(stations);
        }

        // GET: Station/Details/5
        public IActionResult Details(int id)
        {
            var station = _context.Stations.FirstOrDefault(s => s.Id == id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // GET: Station/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Station/Create
        [HttpPost]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = _context.Stations.Find(id);
            if (station != null)
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

