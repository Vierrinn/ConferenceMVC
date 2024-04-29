using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConferenceDomain.Model;
using ConferenceInfrastructure;

namespace ConferenceInfrastructure.Controllers
{
    public class ConferencesController : Controller
    {
        private readonly DbconferenceContext _context;

        public ConferencesController(DbconferenceContext context)
        {
            _context = context;
        }

        // GET: Conferences
        public async Task<IActionResult> Index()
        {
            var dbconferenceContext = _context.Conferences.Include(c => c.Organizer).Include(c => c.Topic);
            return View(await dbconferenceContext.ToListAsync());
        }

        // GET: Conferences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences
                .Include(c => c.Organizer)
                .Include(c => c.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conference == null)
            {
                return NotFound();
            }

            return View(conference);
        }

        // GET: Conferences/Create
        public IActionResult Create()
        {
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "Id", "Login");
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name");
            return View();
        }

        // POST: Conferences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TopicId,Info,Cost,DateTime,Duration,OrganizerId,Id")] Conference conference)
        {
            // Перевірка на унікальність всіх даних конференції
            if (_context.Conferences.Any(c => c.Name == conference.Name && c.DateTime == conference.DateTime && c.OrganizerId == conference.OrganizerId))
            {
                ModelState.AddModelError("", "Конференція з такою назвою, датою та організатором вже існує");
            }
            if (ModelState.IsValid)
            {
                _context.Add(conference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "Id", "Login", conference.OrganizerId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", conference.TopicId);
            return View(conference);
        }

        // GET: Conferences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences.FindAsync(id);
            if (conference == null)
            {
                return NotFound();
            }
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "Id", "Login", conference.OrganizerId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", conference.TopicId);
            return View(conference);
        }

        // POST: Conferences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,TopicId,Info,Cost,DateTime,Duration,OrganizerId,Id")] Conference conference)
        {
            if (id != conference.Id)
            {
                return NotFound();
            }

            // Перевірка на унікальність назви, дати та організатора конференції, ігноруючи поточну конференцію
            if (_context.Conferences.Any(c => c.Id != id && c.Name == conference.Name && c.DateTime == conference.DateTime && c.OrganizerId == conference.OrganizerId))
            {
                ModelState.AddModelError("", "Конференція з такою назвою, датою та організатором вже існує");
            }
            // Форматування ціни без десяткових знаків
            if (conference.Cost.HasValue)
            {
                conference.Cost = Math.Round(conference.Cost.Value, 0);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConferenceExists(conference.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "Id", "Login", conference.OrganizerId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", conference.TopicId);
            return View(conference);
        }

        // GET: Conferences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences
                .Include(c => c.Organizer)
                .Include(c => c.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conference == null)
            {
                return NotFound();
            }

            return View(conference);
        }

        // POST: Conferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conference = await _context.Conferences.FindAsync(id);
            if (conference != null)
            {
                // Видалення всіх пов'язаних записів SignUp
                var signUps = _context.SignUps.Where(s => s.ConferenceId == id);
                _context.SignUps.RemoveRange(signUps);

                _context.Conferences.Remove(conference);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SignUpForConference(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Завантаження конференції разом з організатором
            var conference = await _context.Conferences
                .Include(c => c.Organizer) // Додавання цього рядка
                .FirstOrDefaultAsync(c => c.Id == id);

            if (conference == null || conference.Organizer == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "SignUps", new { conferenceId = conference.Id, conferenceName = conference.Name, conferenceDateTime = conference.DateTime, organizerName = conference.Organizer.Name });
        }



        private bool ConferenceExists(int id)
        {
            return _context.Conferences.Any(e => e.Id == id);
        }
    }
}
