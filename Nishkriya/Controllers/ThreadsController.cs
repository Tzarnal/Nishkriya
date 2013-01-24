using System.Linq;
using System.Web.Mvc;
using Nishkriya.Models;
using Nishkriya.Models.ViewModels;

namespace Nishkriya.Controllers
{
    public class ThreadsController : Controller
    {
        NishkriyaContext db = new NishkriyaContext();

        public ActionResult Index()
        {
            return Page();
        }

        public ActionResult Page(int id=0)
        {
            var pageSize = 20; //Amount of elements on a page

            var threads = db.Threads.OrderBy(thread => thread.Posts.Max(post => post.PostDate));
            var totalPages = threads.Count()/pageSize;
            if (id == 0)
                id = totalPages;

            ViewBag.Title = "All Topics";
            ViewBag.selectedSidebarEntry = "All Topics";
            ViewBag.Paginator = new PaginatorViewModel {PageIndex = id,TotalPages = totalPages, MaximumSpread = 3,Action = "Page",Controller = "Threads"};
 
            var selectedTheads = threads.Skip((id - 1) * pageSize).Take(pageSize).OrderByDescending(thread => thread.Posts.Max(post => post.PostDate));
            return View("Page", selectedTheads);
        }

        public ActionResult LatestTopics()
        {
            var threads = db.Threads.OrderByDescending(thread => thread.Posts.Max(post => post.PostDate)).Take(10);
                                             
            ViewBag.Title = "Latest Topics";
            ViewBag.selectedSidebarEntry = "Latest Topics";

            return View(threads);
        }

        public ActionResult Details(int id)
        {
            var thread = db.Threads.Single(t => t.ThreadId == id);
            ViewBag.Title = thread.Title;

            return View(thread);
        }
    }
}
