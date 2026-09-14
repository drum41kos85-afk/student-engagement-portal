using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Data;
using StudentEngagementPortal.Models;
using StudentEngagementPortal.Models.ViewModels;

namespace StudentEngagementPortal.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var studentId = _userManager.GetUserId(User);
            var messages = await _context.Messages
                .Where(m => m.StudentId == studentId)
                .OrderByDescending(m => m.SubmittedAt)
                .ToListAsync();

            return View(messages);
        }

        [HttpGet]
        public IActionResult Create() => View(new MessageFormViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var studentId = _userManager.GetUserId(User);
            _context.Messages.Add(new Message { StudentId = studentId, Subject = model.Subject, Body = model.Body });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Your message has been submitted.";
            return RedirectToAction(nameof(Index));
        }
    }
}