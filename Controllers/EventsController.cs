using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Abstract;
using MovieApp.Entities;

namespace MovieApp.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventParticipantRepository _eventParticipantRepository;
        private readonly IUserRepository _userRepository;

        public EventsController(IEventRepository eventRepository, IEventParticipantRepository eventParticipantRepository, IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _eventParticipantRepository = eventParticipantRepository;
            _userRepository = userRepository;
        }

        // GET: Events
        public IActionResult Index()
        {
            var events = _eventRepository.Events.Include(e => e.Organizer).ToList();
            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _eventRepository.Events
                .Include(e => e.Organizer)
                .Include(e => e.Participants).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Description,Location,StartDate,EndDate")] Event @event)
        {
            if (User.Identity.IsAuthenticated)
            {
                @event.OrganizerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (ModelState.IsValid)
                {
                    _eventRepository.AddEvent(@event);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }

            var @event = await _eventRepository.Events.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (@event == null)
            {
                return NotFound();
            }

            if (@event.OrganizerId != long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,StartDate,EndDate")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                @event.OrganizerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _eventRepository.UpdateEvent(@event);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attend(long eventId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }

            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var existingParticipant = await _eventParticipantRepository.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (existingParticipant != null)
            {
                // Kullanıcı zaten katılmış
                return RedirectToAction("Details", new { id = eventId });
            }

            var eventParticipant = new EventParticipant
            {
                EventId = eventId,
                UserId = userId
            };

            _eventParticipantRepository.AddEventParticipant(eventParticipant);

            return RedirectToAction("Details", new { id = eventId });
        }

        // Katılım kaldırma
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unattend(long eventId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }

            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventParticipant = await _eventParticipantRepository.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (eventParticipant == null)
            {
                // Kullanıcı zaten katılmamış
                return RedirectToAction("Details", new { id = eventId });
            }

            _eventParticipantRepository.DeleteEventParticipant(eventParticipant);

            return RedirectToAction("Details", new { id = eventId });
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @event = await _eventRepository.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            _eventRepository.DeleteEvent(@event);

            return RedirectToAction(nameof(Index));
        }
        private bool EventExists(long id)
        {
            return _eventRepository.Events.Any(e => e.Id == id);
        }
    }

}

