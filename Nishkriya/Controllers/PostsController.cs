using System.Web.Mvc;
using Nishkriya.Models;
using System.Linq;
using Nishkriya.Models.ViewModels;

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
            var pageSize = 20;

            var posts = db.Posts.OrderBy(p => p.PostDate);
            var totalPages = posts.Count()/pageSize;
            if (id == 0)
                id = totalPages;

            ViewBag.Title = "All Posts";
            ViewBag.selectedSidebarEntry = "All Topics";
            ViewBag.Paginator = new PaginatorViewModel { PageIndex = id, TotalPages = totalPages, MaximumSpread = 3, Action = "Page", Controller = "Posts" };

            var selectedPosts = posts.Skip((id - 1)*pageSize).Take(pageSize).OrderByDescending(p => p.PostDate);
            return View(selectedPosts);

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
