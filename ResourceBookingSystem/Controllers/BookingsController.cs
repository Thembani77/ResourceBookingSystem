using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Data;
using ResourceBookingSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Bookings
        public async Task<IActionResult> Index(DateTime? date, int resourceId = 0)
        {
            var query = _context.Bookings.Include(b => b.Resource).AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(b => b.StartTime.Date == date.Value.Date);
                ViewBag.FilterDate = date.Value.ToString("yyyy-MM-dd");
            }

            if (resourceId > 0)
            {
                query = query.Where(b => b.ResourceId == resourceId);
            }

            ViewBag.ResourceId = new SelectList(await _context.Resources.ToListAsync(), "Id", "Name", resourceId);
            return View(await query.ToListAsync());
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            var resources = await _context.Resources
                .Where(r => r.IsAvailable)
                .ToListAsync();

            ViewBag.ResourceId = new SelectList(resources, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            // Check time validity
            if (booking.EndTime <= booking.StartTime)
            {
                ModelState.AddModelError(nameof(booking.EndTime), "End time must be after start time.");
            }

            // Check for overlapping bookings

            var hasConflict = await _context.Bookings.AnyAsync(b =>
                b.ResourceId == booking.ResourceId &&
                (
                    (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                    (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                    (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)
                ));

            if (hasConflict)
            {
                ModelState.AddModelError(string.Empty, "The selected resource is already booked in this time slot.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Rebuild dropdown if form has errors
            ViewBag.ResourceId = new SelectList(await _context.Resources.ToListAsync(), "Id", "Name", booking.ResourceId);
            return View(booking);
        }
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(m => m.Id == id);

            return booking == null ? NotFound() : View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewBag.ResourceId = new SelectList(await _context.Resources.ToListAsync(), "Id", "Name", booking.ResourceId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            if (id != booking.Id) return NotFound();

            if (booking.EndTime <= booking.StartTime)
            {
                ModelState.AddModelError(nameof(booking.EndTime), "End time must be after start time.");
            }

            var hasConflict = await _context.Bookings.AnyAsync(b =>
        b.Id != booking.Id &&
        b.ResourceId == booking.ResourceId &&
        (
            (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
            (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
            (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)
        ));

            if (hasConflict)
            {
                ModelState.AddModelError(string.Empty, "The selected resource is already booked in this time slot.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                        return NotFound();
                    throw;
                }
            }

            ViewBag.ResourceId = new SelectList(await _context.Resources.ToListAsync(), "Id", "Name", booking.ResourceId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(m => m.Id == id);

            return booking == null ? NotFound() : View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Booking deleted.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Filter
        public async Task<IActionResult> Filter(DateTime? date, int resourceId = 0)
        {
            var query = _context.Bookings.Include(b => b.Resource).AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(b => b.StartTime.Date == date.Value.Date);
                ViewBag.FilterDate = date.Value.ToString("yyyy-MM-dd");
            }

            if (resourceId > 0)
            {
                query = query.Where(b => b.ResourceId == resourceId);
            }

            ViewBag.ResourceId = new SelectList(await _context.Resources.ToListAsync(), "Id", "Name", resourceId);
            return View("Index", await query.ToListAsync());
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
