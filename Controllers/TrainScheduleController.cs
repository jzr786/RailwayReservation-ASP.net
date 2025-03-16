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
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation);
            return View(await schedules.ToListAsync());
        }

        // GET: TrainSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (schedule == null) return NotFound();

            return View(schedule);
        }

        // GET: TrainSchedule/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var trains = _context.Trains.ToList();
            var stations = _context.Stations.ToList();

          
            ViewData["TrainNo"] = new SelectList(trains, "TrainNo", "Name");
            ViewData["FromStationId"] = new SelectList(stations, "Id", "Name");
            ViewData["ToStationId"] = new SelectList(stations, "Id", "Name");

            return View();
        }



        // POST: TrainSchedule/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(TrainSchedule trainSchedule)
        {
            ModelState.Remove("Train");
            ModelState.Remove("FromStation");
            ModelState.Remove("ToStation");

            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns in case of validation error
                ViewData["TrainNo"] = new SelectList(_context.Trains, "TrainNo", "Name", trainSchedule.TrainNo);
                ViewData["FromStationId"] = new SelectList(_context.Stations, "Id", "Name", trainSchedule.FromStationId);
                ViewData["ToStationId"] = new SelectList(_context.Stations, "Id", "Name", trainSchedule.ToStationId);

                return View(trainSchedule);
            }

            _context.Add(trainSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: TrainSchedule/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _context.TrainSchedules.FindAsync(id);
            if (schedule == null) return NotFound();

            ViewData["TrainNo"] = new SelectList(_context.Trains, "TrainNo", "Name", schedule.TrainNo);
            ViewData["FromStationId"] = new SelectList(_context.Stations, "Id", "Name", schedule.FromStationId);
            ViewData["ToStationId"] = new SelectList(_context.Stations, "Id", "Name", schedule.ToStationId);

            return View(schedule);
        }

        // POST: TrainSchedule/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainSchedule trainSchedule)
        {
            if (id != trainSchedule.Id) return NotFound();

            ModelState.Remove("Train");
            ModelState.Remove("FromStation");
            ModelState.Remove("ToStation");

            if (!ModelState.IsValid)
            {
                ViewData["TrainNo"] = new SelectList(_context.Trains, "TrainNo", "Name", trainSchedule.TrainNo);
                ViewData["FromStationId"] = new SelectList(_context.Stations, "Id", "Name", trainSchedule.FromStationId);
                ViewData["ToStationId"] = new SelectList(_context.Stations, "Id", "Name", trainSchedule.ToStationId);

                return View(trainSchedule);
            }

            try
            {
                _context.Update(trainSchedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TrainSchedules.Any(e => e.Id == trainSchedule.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: TrainSchedule/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (schedule == null) return NotFound();

            return View(schedule);
        }

        // POST: TrainSchedule/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation)
                .FirstOrDefaultAsync(ts => ts.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            try
            {
                _context.TrainSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Cannot delete schedule. It may have related reservations.");
                return View(schedule);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainSchedule/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: TrainSchedule/SearchResults
        [HttpPost]
        public async Task<IActionResult> SearchResults(string fromStation, string toStation, DateTime travelDate)
        {
            if (string.IsNullOrEmpty(fromStation) || string.IsNullOrEmpty(toStation))
            {
                ModelState.AddModelError("", "Please select both departure and destination stations.");
                return RedirectToAction("Index", "Home"); // Redirect to the home page if validation fails
            }

            if (travelDate == default)
            {
                ModelState.AddModelError("", "Please select a valid travel date.");
                return RedirectToAction("Index", "Home"); // Redirect to the home page if validation fails
            }

            // Query the database for matching schedules
            var schedules = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation)
                .Where(ts =>
                    ts.FromStation.Name == fromStation &&
                    ts.ToStation.Name == toStation &&
                    ts.DepartureTime.Date == travelDate.Date)
                .ToListAsync();

            // Pass the results to the view
            return View(schedules);
        }

    }
}
