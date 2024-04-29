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
    public class SignUpsController : Controller
    {
        private readonly DbconferenceContext _context;

        public SignUpsController(DbconferenceContext context)
        {
            _context = context;
        }

        // GET: SignUps
        public async Task<IActionResult> Index()
        {
            var dbconferenceContext = _context.SignUps.Include(s => s.Conference).ThenInclude(c => c.Organizer).Include(s => s.User);
            return View(await dbconferenceContext.ToListAsync());
        }

        // GET: SignUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUp = await _context.SignUps
        .Include(s => s.Conference)
            .ThenInclude(c => c.Topic)
        .Include(s => s.Conference)
            .ThenInclude(c => c.Organizer)
        .Include(s => s.User)
        .FirstOrDefaultAsync(m => m.Id == id);
            if (signUp == null)
            {
                return NotFound();
            }

            return View(signUp);
        }

         // GET: SignUps/Create
         public IActionResult Create()
         {
             ViewData["ConferenceId"] = new SelectList(_context.Conferences.Select(c => new
             {
                 Id = c.Id,
                 Description = $"{c.Name}, {c.DateTime.ToString("g")}, {c.Organizer.Name}"
             }), "Id", "Description");
             ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login");
             return View();
         }

 /*        [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> Create([Bind("Id,UserId,Password,ConferenceId")] SignUp signUp)
 {
             // Перевірка на існування користувача
             var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == signUp.UserId.ToString() && signUp.UserId != null);
             if (existingUser == null)
     {
         ModelState.AddModelError("", "Користувач з таким логіном не існує");
     }

     if (ModelState.IsValid)
     {
         _context.Add(signUp);
         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
     }
     ViewData["UserId"] = new SelectList(_context.Users, "Login", "Login", signUp.UserId);
     return View(signUp);
 }
        */
        // GET: SignUps/Create
       /* public IActionResult Create(int conferenceId, string conferenceName, DateTime conferenceDateTime, string organizerName)
        {
            // Створення моделі SignUpViewModel з переданими параметрами
            var model = new SignUp
            {
                ConferenceId = conferenceId,
                ConferenceName = conferenceName,
                ConferenceDateTime = conferenceDateTime,
                OrganizerName = organizerName
            };
            return View(model);
        }

        // POST: SignUps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUp model)
        {
            if (ModelState.IsValid)
            {
                // Перевірка на існування користувача
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (existingUser == null)
                {
                    ModelState.AddModelError("", "Користувач з таким логіном або паролем не існує");
                    return View(model);
                }

                // Створення нового запису SignUp
                var signUp = new SignUp
                {
                    UserId = existingUser.Id,
                    ConferenceId = model.ConferenceId
                };

                _context.Add(signUp);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Conferences");
            }
            return View(model);
        }*/





          // POST: SignUps/Create
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("Id,UserId,ConferenceId")] SignUp signUp)
          {
              // Перевірка на унікальність комбінації UserId та ConferenceId
              if (_context.SignUps.Any(s => s.UserId == signUp.UserId && s.ConferenceId == signUp.ConferenceId))
              {
                  ModelState.AddModelError("", "Користувач вже зареєстрований на цю конференцію");
              }
              if (ModelState.IsValid)
              {
                  _context.Add(signUp);
                  await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              }
              ViewData["ConferenceId"] = new SelectList(_context.Conferences.Select(c => new
              {
                  Id = c.Id,
                  Description = $"{c.Name}, {c.DateTime.ToString("g")}, {c.Organizer.Name}"
              }), "Id", "Description", signUp.ConferenceId);
              ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", signUp.UserId);
              return View(signUp);
          }
        

        // GET: SignUps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUp = await _context.SignUps.FindAsync(id);
            if (signUp == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences.Select(c => new
            {
                Id = c.Id,
                Description = $"{c.Name}, {c.DateTime.ToString("g")}, {c.Organizer.Name}"
            }), "Id", "Description");
            //ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", signUp.ConferenceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", signUp.UserId);
            return View(signUp);
        }

        // POST: SignUps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ConferenceId")] SignUp signUp)
        {
            if (id != signUp.Id)
            {
                return NotFound();
            }
            // Перевірка на унікальність комбінації UserId та ConferenceId

            if (_context.SignUps.Any(s => s.UserId == signUp.UserId && s.ConferenceId == signUp.ConferenceId))
            {
                ModelState.AddModelError("", "Користувач вже зареєстрований на цю конференцію");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(signUp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignUpExists(signUp.Id))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences.Select(c => new
            {
                Id = c.Id,
                Description = $"{c.Name}, {c.DateTime.ToString("g")}, {c.Organizer.Name}"
            }), "Id", "Description", signUp.ConferenceId);

            // ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", signUp.ConferenceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", signUp.UserId);
            return View(signUp);
        }

        // GET: SignUps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signUp = await _context.SignUps
                .Include(s => s.Conference)
                    .ThenInclude(c => c.Topic)
                .Include(s => s.Conference)
                    .ThenInclude(c => c.Organizer)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signUp == null)
            {
                return NotFound();
            }

            return View(signUp);
        }

        // POST: SignUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signUp = await _context.SignUps.FindAsync(id);
            if (signUp != null)
            {
                _context.SignUps.Remove(signUp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SignUpExists(int id)
        {
            return _context.SignUps.Any(e => e.Id == id);
        }
    }
}
