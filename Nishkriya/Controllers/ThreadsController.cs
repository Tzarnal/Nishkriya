using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Controllers
{
    public class ThreadsController : Controller
    {
        private ThreadDbContext db = new ThreadDbContext();

        //
        // GET: /Threads/

        public ActionResult Index()
        {
            return View(db.Threads);
        }


        //
        // GET: /Threads/id

        public ActionResult Details(int id)
        {
            ViewBag.id = id;
            return View(db.Threads);
        }
    }
}
