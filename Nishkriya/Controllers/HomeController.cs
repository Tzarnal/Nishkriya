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
            return View(db.Stats);
        }
    }
}
