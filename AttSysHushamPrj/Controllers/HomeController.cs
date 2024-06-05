using AttSysHushamPrj.Data;
using AttSysHushamPrj.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace AttSysHushamPrj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        private  UserManager<ApplicationUser> _userManager;
          

        public IActionResult Index(string returnUrl = null)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            //if(userId == null)
            //{
            //    returnUrl = returnUrl ?? Url.Content("Identity/Account/Login");
            //    return LocalRedirect(returnUrl);
            //}
            TimeSpan timeIn = new TimeSpan(0, 0, 0);
            TimeSpan timeOut = new TimeSpan(0, 0, 0);
            int totalAttandanceValioation ;
            // var userName = User.FindFirstValue(ClaimTypes.Name);
            ApplicationUser user = _context.Users.Where(c => c.Id == userId).SingleOrDefault();
           
            
            List<Event> eventlist = _context.Event.Where(c => c.BadgeNum == user.BadgeNumber).ToList();

            if (eventlist.Where(c => c.EventDate.Date == DateTime.Now.Date && c.EventType == (int)EventTypes.IN).ToList().Count > 0)
            {
                timeIn = (from r in eventlist
                          where r.EventDate.Date == DateTime.Now.Date && r.EventType == (int)EventTypes.IN
                          select r.EventTime).SingleOrDefault();
                ViewBag.CheckIn = timeIn.ToString(@"hh\:mm\:ss");
            }
                          
            else
                ViewBag.CheckInbool = true;

            if (eventlist.Where(c => c.EventDate.Date == DateTime.Now.Date && c.EventType == (int)EventTypes.OUT).ToList().Count > 0)
            {
                timeIn = (from r in eventlist
                          where r.EventDate.Date == DateTime.Now.Date && r.EventType == (int)EventTypes.OUT
                          select r.EventTime).SingleOrDefault();
                ViewBag.CheckOut = timeIn.ToString(@"hh\:mm\:ss");
            }

            else
                ViewBag.CheckOutbool = true;

            if (eventlist.Where(c => c.IsEarly == true || c.IsLate == true) != null)
            {
                totalAttandanceValioation = eventlist.Where(c => c.IsEarly == true || c.IsLate == true).ToList().Count;
                ViewBag.totalAttandanceValioation = totalAttandanceValioation.ToString();
            }

            else
                ViewBag.totalAttandanceValioation = "0";


            
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Home/Index");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _context.Users.Where(c => c.Id == userId).SingleOrDefault();

            var istoday = DateTime.Now;
            //select today event 
            Event eventObj = _context.Event.Where(c => c.BadgeNum == user.BadgeNumber && c.EventDate.Date == istoday.Date && c.EventType == (int)EventTypes.IN).SingleOrDefault();
            if (eventObj != null)
            {
                //var qur = ( from eve in _context.Event
                //            where eve.EventDate == istoday
                return LocalRedirect(returnUrl);
            }
            else
            {
                Event obj = new Event
                {
                    BadgeNum = user.BadgeNumber,
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
                return LocalRedirect(returnUrl);
            }


        }

        [HttpPost]
        public ActionResult CheckOut(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Home/Index");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = _context.Users.Where(c => c.Id == userId).SingleOrDefault();

            var istoday = DateTime.Now;
            //select today event 
            Event eventObj = _context.Event.Where(c => c.BadgeNum == user.BadgeNumber && c.EventDate.Date == istoday.Date && c.EventType == (int)EventTypes.OUT).SingleOrDefault();
            if (eventObj != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                Event obj = new Event
                {
                    BadgeNum = user.BadgeNumber,
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
                return LocalRedirect(returnUrl);
            }


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
