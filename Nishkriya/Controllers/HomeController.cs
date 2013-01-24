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
            ViewBag.posts = db.Posts.OrderByDescending(p => p.PostDate).Take(10);
            ViewBag.selectedSidebarEntry = "Latest Posts"; 

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
