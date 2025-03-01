using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Data;
using RailwayReservation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{
    [Authorize(Roles = "Customer")] // Ensures only authenticated users can manage cancellations
    public class CancellationController : Controller
    {
        private readonly RailwayContext _context;

        public CancellationController(RailwayContext context)
        {
            _context = context;
        }

        // GET: Cancellation
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cancellations.ToListAsync());
        }

        // GET: Cancellation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cancellation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cancellation cancellation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cancellation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cancellation);
        }

        // GET: Cancellation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cancellation = await _context.Cancellations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancellation == null) return NotFound();

            return View(cancellation);
        }

        // GET: Cancellation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cancellation = await _context.Cancellations.FindAsync(id);
            if (cancellation == null) return NotFound();

            return View(cancellation);
        }

        // POST: Cancellation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cancellation cancellation)
        {
            if (id != cancellation.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(cancellation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cancellation);
        }

        // GET: Cancellation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cancellation = await _context.Cancellations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancellation == null) return NotFound();

            return View(cancellation);
        }

        // POST: Cancellation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cancellation = await _context.Cancellations.FindAsync(id);
            if (cancellation != null)
            {
                _context.Cancellations.Remove(cancellation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
