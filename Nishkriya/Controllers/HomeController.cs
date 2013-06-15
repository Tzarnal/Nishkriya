using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nishkriya.Feeds;
using Nishkriya.Models;
using Nishkriya.Models.Builders;

namespace Nishkriya.Controllers
{
    public class HomeController : Controller
    {
        readonly NishkriyaContext _db = new NishkriyaContext();

        public ActionResult Index()
        {
            DateTime sessionTimeSinceLastVisit;
            DateTime timeSinceLastVisit = Request.Cookies["TimeSinceLastVisit"] != null ? DateTime.Parse(Request.Cookies["TimeSinceLastVisit"].Value, CultureInfo.InvariantCulture) : DateTime.UtcNow.AddHours(-8);

            //If there is a timeSinceLastVisit cookie then store that value in a session cookie and use that to display new content
            //If there isn't use the last 8 hours. 
            //Always update the TimesinceLastVisit cookie so the last visit is accurate
            //Keep the session cookie the same if it already exists
                       
            if (Request.Cookies["SessionTimeSinceLastVisit"] != null)
            {
                sessionTimeSinceLastVisit = DateTime.Parse(Request.Cookies["SessionTimeSinceLastVisit"].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                sessionTimeSinceLastVisit = timeSinceLastVisit;
                Response.Cookies.Add(SessionTimeSinceLastVisitCookie(sessionTimeSinceLastVisit));
            }

            Response.Cookies.Add(TimeSinceLastVisitCookie());

            var viewModel = new NewContentViewModelBuilder(_db).Build(Request.Cookies["LatestPostsExplanationDismissed"] != null, sessionTimeSinceLastVisit);            
            ViewBag.Title = "New Content";
            ViewBag.selectedSidebarEntry = "New Content";

            if(!viewModel.NewContent)
            {
                return View("NoNewContent");
            }

            return View(viewModel);
        }

        public ActionResult MarkAsRead(DateTime? markTime = null)
        {
            if (markTime == null)
                markTime = DateTime.UtcNow;
            
            Response.Cookies.Add(TimeSinceLastVisitCookie());
            Response.Cookies.Add(SessionTimeSinceLastVisitCookie(markTime.Value));
            
            Response.Redirect(Url.Action("Index"));
            return View("NoNewContent");
        }

        public ActionResult DismissExplanation()
        {
            Response.Cookies.Add(ExplanationDismissedCookie());

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
            return View(_db.Stats.Where(s => s.Start > today).OrderByDescending(s => s.Start));
        }

        [HttpGet]
        public FeedResult Feed()
        {
            if (Request == null)
            {
                throw new ArgumentNullException("Request");
            }

            return new FeedResult(new FeedBuilder().Feed(Request.Url.AbsoluteUri, Url));
        }


        private HttpCookie TimeSinceLastVisitCookie()
        {
            var cookie = new HttpCookie("TimeSinceLastVisit", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))
                {
                    Expires = DateTime.Now.AddDays(30)
                };

            return cookie;
        }

        private HttpCookie SessionTimeSinceLastVisitCookie(DateTime lastVisit)
        {
            var cookie = new HttpCookie("SessionTimeSinceLastVisit", lastVisit.ToString(CultureInfo.InvariantCulture))
                {
                    Expires = DateTime.MinValue
                };

            return cookie;
        }

        private HttpCookie ExplanationDismissedCookie()
        {
            var cookie = new HttpCookie("LatestPostsExplanationDismissed", "true")
                {
                    Expires = DateTime.MaxValue
                };

            return cookie;
        }
    }
}
