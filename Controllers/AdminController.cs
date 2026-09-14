using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Data;
using StudentEngagementPortal.Models;
using StudentEngagementPortal.Models.ViewModels;

namespace StudentEngagementPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Events = await _context.Events.OrderBy(e => e.StartsAt).ToListAsync();
            ViewBag.OpenMessages = await _context.Messages
                .Where(m => m.Status == MessageStatus.Open)
                .OrderBy(m => m.SubmittedAt)
                .ToListAsync();
            return View();
        }

        [HttpGet]
        public IActionResult CreateEvent() => View("EventForm", new EventFormViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(EventFormViewModel model)
        {
            if (!ModelState.IsValid) return View("EventForm", model);

            _context.Events.Add(new Event
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                StartsAt = model.StartsAt,
                MaxParticipants = model.MaxParticipants
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            return View("EventForm", new EventFormViewModel
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Category = ev.Category,
                StartsAt = ev.StartsAt,
                MaxParticipants = ev.MaxParticipants
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(EventFormViewModel model)
        {
            if (!ModelState.IsValid) return View("EventForm", model);

            var ev = await _context.Events.FindAsync(model.Id);
            if (ev == null) return NotFound();

            ev.Title = model.Title;
            ev.Description = model.Description;
            ev.Category = model.Category;
            ev.StartsAt = model.StartsAt;
            ev.MaxParticipants = model.MaxParticipants;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SignupList(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpGet]
        public IActionResult PostAnnouncement() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAnnouncement(string title, string body)
        {
            var adminId = _userManager.GetUserId(User);
            _context.Announcements.Add(new Announcement { Title = title, Body = body, PostedByAdminId = adminId });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                message.Status = MessageStatus.Resolved;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}