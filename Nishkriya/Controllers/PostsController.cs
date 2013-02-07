using System;
using System.Web.Mvc;
using Nishkriya.Models;
using System.Linq;
using Nishkriya.Models.ViewModels;
using Nishkriya.Properties;

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
            var pageSize = Settings.Default.PageSize;
            var posts = db.Posts.OrderByDescending(p => p.PostDate);
            
            var totalPages = (int)Math.Ceiling(posts.Count() / (float)pageSize);

            ViewBag.Title = "All Posts";
            ViewBag.selectedSidebarEntry = "All Posts";
            ViewBag.Paginator = new PaginatorViewModel
                {
                    PageIndex = id == 0 ? 1 : id,
                    TotalPages = totalPages == 0 ? 1 : totalPages,
                    MaximumSpread = 3,
                    Action = "Page",
                    Controller = "Posts"
                };

            var selectedPosts = posts.Skip((id - 1)*pageSize).Take(pageSize).OrderByDescending(p => p.PostDate);
            return View("Page",selectedPosts);

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

            ViewBag.Title = "Specific Post";

            return View(post);
        }
    }
}
