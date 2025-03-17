using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Data;
using RailwayReservation.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{
    public class ReservationController : Controller
    {
        

        private readonly RailwayContext _context;

        public ReservationController(RailwayContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); 
            }

            var reservations = await _context.Reservations
                .Include(r => r.TrainSchedule)
                .ThenInclude(ts => ts.Train)
                .Include(r => r.FromStation)
                .Include(r => r.ToStation)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(reservations);
        }

        
        [Authorize(Roles = "Customer")]
        public IActionResult SelectTrain()
        {
            var trains = _context.Trains.ToList();
            ViewBag.Trains = new SelectList(trains, "TrainNo", "Name");
            return View();
        }

        
        [HttpPost]
        public IActionResult SelectTrain(int trainNo)
        {
            return RedirectToAction("SelectSchedule", new { trainNo });
        }

        
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SelectSchedule(int trainNo)
        {
            var train = await _context.Trains
        .FirstOrDefaultAsync(t => t.TrainNo == trainNo);

            if (train == null)
            {
                ViewBag.Message = "Train not found.";
                return View();
            }

            var schedules = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation)
                .Where(ts => ts.TrainNo == trainNo)
                .ToListAsync();

            if (schedules == null || !schedules.Any())
            {
                ViewBag.Message = "No schedules available for the selected train.";
                ViewBag.TrainName = train.Name;
                return View();
            }

            ViewBag.TrainNo = trainNo;
            ViewBag.TrainName = train.Name;
            return View(schedules);
        }

        
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(int trainScheduleId)
        {
            var schedule = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation)
                .FirstOrDefaultAsync(ts => ts.Id == trainScheduleId);

            if (schedule == null)
            {
                return NotFound();
            }

            ViewBag.TrainSchedule = schedule;
            return View();
        }

        
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(int trainScheduleId, int numberOfSeats, string seatType)
        {
            var schedule = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Include(ts => ts.FromStation)
                .Include(ts => ts.ToStation)
                .FirstOrDefaultAsync(ts => ts.Id == trainScheduleId);

            if (schedule == null)
            {
                return NotFound();
            }

            
            int availableSeats = 0;
            decimal farePerKm = 0;

            switch (seatType)
            {
                case "AC1":
                    availableSeats = schedule.AC1Seats;
                    farePerKm = schedule.AC1FarePerKm;
                    break;
                case "AC3":
                    availableSeats = schedule.AC3Seats;
                    farePerKm = schedule.AC3FarePerKm;
                    break;
                case "Sleeper":
                    availableSeats = schedule.SleeperSeats;
                    farePerKm = schedule.SleeperFarePerKm;
                    break;
                default:
                    ModelState.AddModelError("", "Invalid seat type selected.");
                    ViewBag.TrainSchedule = schedule;
                    return View();
            }

            if (numberOfSeats > availableSeats)
            {
                ModelState.AddModelError("", "Not enough seats available for the selected class.");
                ViewBag.TrainSchedule = schedule;
                return View();
            }

            
            decimal totalFare = schedule.Distance * farePerKm * numberOfSeats;

            
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); 
            }

            var reservation = new Reservation
            {
                TrainScheduleId = trainScheduleId,
                UserId = userId,
                FromStationId = schedule.FromStationId,
                ToStationId = schedule.ToStationId,
                NumberOfSeats = numberOfSeats,
                ReservationTime = DateTime.Now,
                TotalFare = totalFare,
                SeatType = seatType 
            };

            
            switch (seatType)
            {
                case "AC1":
                    schedule.AC1Seats -= numberOfSeats;
                    break;
                case "AC3":
                    schedule.AC3Seats -= numberOfSeats;
                    break;
                case "Sleeper":
                    schedule.SleeperSeats -= numberOfSeats;
                    break;
            }

            _context.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { id = reservation.Id });
        }

        
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Confirmation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.TrainSchedule)
                .ThenInclude(ts => ts.Train)
                .Include(r => r.FromStation)
                .Include(r => r.ToStation)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.TrainSchedule)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            
            switch (reservation.SeatType)
            {
                case "AC1":
                    reservation.TrainSchedule.AC1Seats += reservation.NumberOfSeats;
                    break;
                case "AC3":
                    reservation.TrainSchedule.AC3Seats += reservation.NumberOfSeats;
                    break;
                case "Sleeper":
                    reservation.TrainSchedule.SleeperSeats += reservation.NumberOfSeats;
                    break;
                default:
                    
                    break;
            }

            
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PrintTicket(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.TrainSchedule)
                .ThenInclude(ts => ts.Train)
                .Include(r => r.FromStation)
                .Include(r => r.ToStation)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return new ViewAsPdf("Ticket", reservation)
            {
                FileName = $"Ticket_{reservation.Id}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--no-stop-slow-scripts --javascript-delay 1000"
            };
        }
    }
}