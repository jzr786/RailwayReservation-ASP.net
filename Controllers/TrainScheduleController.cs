using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Data;
using RailwayReservation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{
    [Authorize(Roles = "Admin")] // Ensures only authenticated users can manage schedules
    public class TrainScheduleController : Controller
    {
        private readonly RailwayContext _context;

        public TrainScheduleController(RailwayContext context)
        {
            _context = context;
        }

        // GET: TrainSchedule
        public async Task<IActionResult> Index()
        {
            var schedules = _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.Station);
            return View(await schedules.ToListAsync());
        }

        // GET: TrainSchedule/Create
        public IActionResult Create()
        {
            ViewData["TrainNo"] = new SelectList(_context.Trains, "TrainNo", "Name");
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name");
            return View();
        }

        // POST: TrainSchedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainSchedule trainSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TrainNo"] = new SelectList(_context.Trains, "TrainNo", "Name", trainSchedule.TrainNo);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", trainSchedule.StationId);
            return View(trainSchedule);
        }

        // GET: TrainSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null) return NotFound();

            return View(schedule);
        }

        // GET: TrainSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _context.TrainSchedules.FindAsync(id);
            if (schedule == null) return NotFound();

            ViewData["TrainNo"] = new SelectList(_context.Trains, "TrainNo", "Name", schedule.TrainNo);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", schedule.StationId);
            return View(schedule);
        }

        // POST: TrainSchedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainSchedule trainSchedule)
        {
            if (id != trainSchedule.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(trainSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TrainNo"] = new SelectList(_context.Trains, "TrainNo", "Name", trainSchedule.TrainNo);
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", trainSchedule.StationId);
            return View(trainSchedule);
        }

        // GET: TrainSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null) return NotFound();

            return View(schedule);
        }

        // POST: TrainSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.TrainSchedules.FindAsync(id);
            if (schedule != null)
            {
                _context.TrainSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
