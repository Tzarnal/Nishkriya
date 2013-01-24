using System.Web.Mvc;
using Nishkriya.Models;
using System.Linq;

namespace Nishkriya.Controllers
{
    public class PostsController : Controller
    {
        NishkriyaContext db = new NishkriyaContext();
        
        public ActionResult Index()
        {
            return Page();
        }

        public ActionResult Page(int id = 0)
        {
            return View();

        }

        public ActionResult LatestPosts()
        {
            var posts = db.Posts.OrderByDescending(p => p.PostDate).Take(10);
            ViewBag.selectedSidebarEntry = "Latest Posts";
            ViewBag.Title = "Latest Posts";

            return View(posts);
        }

        public ActionResult Details(int id)
        {
            var post = db.Posts.FirstOrDefault(p => p.Id == id);
            
            return View(post);
        }
    }
}
