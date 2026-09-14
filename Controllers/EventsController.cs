using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Data;
using StudentEngagementPortal.Models;
using StudentEngagementPortal.Models.ViewModels;

namespace StudentEngagementPortal.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string category)
        {
            var currentUserId = _userManager.GetUserId(User);
            var query = _context.Events.AsQueryable();
            if (!string.IsNullOrEmpty(category))
                query = query.Where(e => e.Category == category);

            var events = await query
                .OrderBy(e => e.StartsAt)
                .Select(e => new EventListItemViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Category = e.Category,
                    StartsAt = e.StartsAt,
                    MaxParticipants = e.MaxParticipants,
                    SpotsTaken = e.Registrations.Count,
                    IsRegisteredByCurrentUser = currentUserId != null && e.Registrations.Any(r => r.StudentId == currentUserId)
                })
                .ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ev = await _context.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int id)
        {
            var studentId = _userManager.GetUserId(User);
            var ev = await _context.Events.Include(e => e.Registrations).FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            bool alreadyRegistered = ev.Registrations.Any(r => r.StudentId == studentId);
            bool isFull = ev.Registrations.Count >= ev.MaxParticipants;

            if (!alreadyRegistered && !isFull)
            {
                _context.Registrations.Add(new Registration { EventId = id, StudentId = studentId });
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "You're registered for this event.";
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "You are already registered, or the event just filled up.";
                }
            }
            else
            {
                TempData["Error"] = isFull ? "This event is full." : "You're already registered.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unregister(int id)
        {
            var studentId = _userManager.GetUserId(User);
            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.EventId == id && r.StudentId == studentId);

            if (registration != null)
            {
                _context.Registrations.Remove(registration);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registration cancelled.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}