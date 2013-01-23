using System.Linq;
using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Controllers
{
    public class ThreadsController : Controller
    {
        NishkriyaContext db = new NishkriyaContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LatestTopics()
        {
            return View();
        }

        public ActionResult Details(int id)
        {

            var thread = db.Threads.Single(t => t.ThreadId == id);

            ViewBag.Title = thread.Title;
            return View(thread);
        }
    }
}
