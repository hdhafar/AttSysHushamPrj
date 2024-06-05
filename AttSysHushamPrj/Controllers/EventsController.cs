using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttSysHushamPrj.Data;
using AttSysHushamPrj.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AttSysHushamPrj.Controllers
{

    public class EventsController : Controller
    {
        private  ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _context.Users.Where(c => c.Id == userId).SingleOrDefault();

            return View(await _context.Event.Where(c => c.BadgeNum == user.BadgeNumber).ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [HttpPost]
        public ActionResult CheckIn()
        {
            
            var istoday = DateTime.Now;
            //select today event 
            Event eventObj = _context.Event.Where(c => c.BadgeNum == 3333 && c.EventDate.Date == istoday.Date && c.EventType == (int)EventTypes.IN).SingleOrDefault();
            if(eventObj != null)
            {
                //var qur = ( from eve in _context.Event
                //            where eve.EventDate == istoday
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Event obj = new Event
                {
                    BadgeNum = 3333,
                    EventDate = istoday,
                    EventTime = (TimeSpan)istoday.TimeOfDay,
                    EventType = (int)EventTypes.IN
                };
                if ((TimeSpan)istoday.TimeOfDay > new TimeSpan(8, 30, 0))
                {
                    obj.IsLate = true;
                }
                else
                {
                    obj.IsLate = false;

                }
                _context.Event.Add(obj);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
          
          
        }

        [HttpPost]
        public ActionResult CheckOut()
        {

            var istoday = DateTime.Now;
            //select today event 
            Event eventObj = _context.Event.Where(c => c.BadgeNum == 3333 && c.EventDate.Date == istoday.Date && c.EventType == (int)EventTypes.OUT).SingleOrDefault();
            if (eventObj != null)
            {
                 return RedirectToAction(nameof(Index));
            }
            else
            {
                Event obj = new Event
                {
                    BadgeNum = 3333,
                    EventDate = istoday,
                    EventTime = (TimeSpan)istoday.TimeOfDay,
                    EventType = (int)EventTypes.OUT
                };
                if ((TimeSpan)istoday.TimeOfDay < new TimeSpan(15, 0, 0))
                {
                    obj.IsEarly = true;
                }
                else
                {
                    obj.IsEarly = false;

                }
                _context.Event.Add(obj);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }


        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventID,EventDate,EventTime,EventType")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.EventID = Guid.NewGuid();
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EventID,Justification")] Event @event)
        {
            if (id != @event.EventID)
            {
                return NotFound();
            }
            Event old = _context.Event.Where(C => C.EventID == id).SingleOrDefault();
            old.Justification = @event.Justification;

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventID))
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
            return View(old);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var @event = await _context.Event.FindAsync(id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(Guid id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }
    }
}
