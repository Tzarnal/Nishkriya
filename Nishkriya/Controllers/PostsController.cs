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

        public ActionResult Page(int id = 1)
        {
            var pageSize = Settings.Default.PostsPerPage;
            var posts = db.Posts.OrderByDescending(p => p.PostDate);
            
            var totalPages = (int)Math.Ceiling(posts.Count() / (float)pageSize);

            ViewBag.Title = "All Posts";
            ViewBag.selectedSidebarEntry = "All Posts";
            ViewBag.Paginator = new PaginatorViewModel
                {
                    PageIndex = id,
                    TotalPages = totalPages == 0 ? 1 : totalPages,
                    MaximumSpread = 3,
                    Action = "Page",
                    Controller = "Posts"
                };

            var selectedPosts = posts.Skip((id - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToViewModels()
                                     .OrderByDescending(p => p.PostDate)
                                     .ToList();

            return View("Page",selectedPosts);
        }

        public ActionResult LatestPosts()
        {
            var posts = db.Posts.OrderByDescending(p => p.PostDate)
                                .Take(10)
                                .ToViewModels()
                                .ToList();

            ViewBag.selectedSidebarEntry = "Latest Posts";
            ViewBag.Title = "Latest Posts";

            return View(posts);
        }

        public ActionResult Details(int id)
        {
            var post = db.Posts.Where(p => p.Id == id);

            if (!post.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Title = "Specific Post";

            return View(post.ToViewModels().First());
        }
    }
}
