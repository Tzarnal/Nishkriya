using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Controllers
{
    public class HomeController : Controller
    {
        readonly NishkriyaContext db = new NishkriyaContext();

        public ActionResult Index()
        {
            DateTime sessionTimeSinceLastVisit;
            DateTime timeSinceLastVisit = Request.Cookies["TimeSinceLastVisit"] != null ? DateTime.Parse(Request.Cookies["TimeSinceLastVisit"].Value) : DateTime.UtcNow.AddHours(-8);

            //If there is a timeSinceLastVisit cookie then store that value in a session cookie and use that to display new content
            //If there isn't use the last 8 hours. 
            //Always update the TimesinceLastVisit cookie so the last visit is accurate
            //Keep the session cookie the same if it already exists
                       
            if (Request.Cookies["SessionTimeSinceLastVisit"] != null)
            {
                sessionTimeSinceLastVisit = DateTime.Parse(Request.Cookies["SessionTimeSinceLastVisit"].Value);
            }
            else
            {
                sessionTimeSinceLastVisit = timeSinceLastVisit;
                Response.Cookies.Add(SessionTimeSinceLastVisitCookie(sessionTimeSinceLastVisit));
            }

            Response.Cookies.Add(TimeSinceLastVisitCookie());

            ViewBag.Title = "New Content";
            ViewBag.selectedSidebarEntry = "New Content";
            ViewBag.SessionTimeSinceLastVisit = sessionTimeSinceLastVisit;

            var threads = db.Threads.OrderByDescending(thread => thread.Posts.Max(post => post.PostDate)).Where(thread => thread.Posts.Max(post => post.PostDate) > sessionTimeSinceLastVisit);

            if(!threads.Any())
            {
                return View("NoNewContent");
            }
            return View(threads);
        }

        public ActionResult MarkAsRead()
        {
            Response.Cookies.Add(TimeSinceLastVisitCookie());
            Response.Cookies.Add(SessionTimeSinceLastVisitCookie(DateTime.UtcNow));
            
            Response.Redirect(Url.Action("Index"));
            return View("NoNewContent");
        }

        public ActionResult About()
        {
            ViewBag.selectedSidebarEntry = "About";
            ViewBag.Title = "About";
                 
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.selectedSidebarEntry = "Contact";
            ViewBag.Title = "Contact";

            return View();
        }

        [HttpGet]
        public ActionResult Stats()
        {
            var today = DateTime.Now.Date;
            return View(db.Stats.Where(s => s.Start > today).OrderByDescending(s => s.Start));
        }

        private HttpCookie TimeSinceLastVisitCookie()
        {
            var cookie = new HttpCookie("TimeSinceLastVisit", DateTime.UtcNow.ToString() )
                             {Expires = DateTime.Now.AddDays(30)};

            return cookie;
        }

        private HttpCookie SessionTimeSinceLastVisitCookie(DateTime lastVisit)
        {
            var cookie = new HttpCookie("SessionTimeSinceLastVisit", lastVisit.ToString() )
                             {Expires = DateTime.MinValue};

            return cookie;
        }
    }
}
