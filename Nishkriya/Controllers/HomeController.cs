using System;
using System.Linq;
using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Controllers
{
    public class HomeController : Controller
    {
        readonly NishkriyaContext db = new NishkriyaContext();

        public ActionResult Index()
        {
            return RedirectToAction("LatestTopics", "Threads");
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
    }
}
