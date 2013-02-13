using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using Nishkriya.Models;
using Nishkriya.Models.Builders;
using Nishkriya.Results;

namespace Nishkriya.Controllers
{
    public class HomeController : Controller
    {
        readonly NishkriyaContext _db = new NishkriyaContext();

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

            var viewModel = new NewContentViewModelBuilder(_db).Build(Request.Cookies["LatestPostsExplanationDismissed"] != null, sessionTimeSinceLastVisit);            
            ViewBag.Title = "New Content";
            ViewBag.selectedSidebarEntry = "New Content";

            if(!viewModel.newContent)
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

            var cssUri = new UriBuilder(Request.Url.AbsoluteUri)
                {
                    // ReSharper disable Html.PathError - the bundler takes care of creating this path for us
                    Path = Url.Content("~/bundles/css")
                    // ReSharper restore Html.PathError
                }.Uri;

            var items = _db.Posts
                        .OrderByDescending(p => p.PostDate)
                        .Take(20).ToList().Select(post => new SyndicationItem(
                        String.Format("{0} - {1} - {2}",
                            post.ForumAccount.Name,
                            post.Thread.Title,
                            post.Id),
                        String.Format("<link href='{0}' rel='stylesheet' /><br />{1}", cssUri, HttpUtility.HtmlDecode(post.Content)),
                        new UriBuilder(Request.Url.AbsoluteUri)
                            {
                                Path = Url.Action("Details", "Posts", new { id = post.Id })
                            }.Uri))
                    .ToList();

            var feed = new SyndicationFeed("Nishkriya - Latest Posts", "An Exalted developer / writer tracker", new Uri(Request.Url.AbsoluteUri), items)
                {
                    Language = "en-US",
                    LastUpdatedTime = items.Max(i => i.PublishDate)
                };
            return new FeedResult(new Rss20FeedFormatter(feed));
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
