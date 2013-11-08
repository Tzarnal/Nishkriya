using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Nishkriya.Extensions;
using Nishkriya.Models;
using Nishkriya.Models.ViewModels;
using Nishkriya.Properties;

namespace Nishkriya.Controllers
{
    public class AccountsController : Controller
    {
        private readonly NishkriyaContext db = new NishkriyaContext();

        //
        // GET: /ForumAccounts/

        public ActionResult Index()
        {
            ViewBag.selectedSidebarEntry = "Posters";
            if(User.Identity.IsAuthenticated)
            {
                ViewBag.Title = "Edit Posters";
                return View(db.Accounts.ToList());    
            }

            ViewBag.Title = "Posters";
            return View("FancyIndex", db.Accounts.ToList());
        }

        //
        // GET: /ForumAccounts/Details/5

        public ActionResult Details(int id = 0, int page = 0)
        {
            ForumAccount forumaccount = db.Accounts.Find(id);
            
            if (forumaccount == null)
            {
                return HttpNotFound();
            }

            var pageSize = Settings.Default.PostsPerPage;
            var posts = forumaccount.Posts.OrderByDescending(p => p.PostDate);

            var totalPages = (int) Math.Ceiling(posts.Count()/(float) pageSize);
            if (totalPages == 0)
                totalPages = 1;

            if (page == 0)
                page = 1;

            ViewBag.AccountId = forumaccount.Id;
            ViewBag.Title = forumaccount.Name;
            ViewBag.selectedSidebarEntry = forumaccount.Name;

            ViewBag.Paginator = new PaginatorViewModel
                {
                    PageIndex = page,
                    TotalPages = totalPages,
                    MaximumSpread = 3,
                    Action = "Details",
                    Controller = "Accounts",
                    ContentId = id
                };

            var selectedPosts = posts.Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToViewModels()
                                     .OrderByDescending(p => p.PostDate)
                                     .ToList();

            return View(selectedPosts);
        }

        //
        // GET: /ForumAccounts/Create

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ForumAccounts/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(ForumAccount forumaccount)
        {
            if (ModelState.IsValid)
            {
                forumaccount.YafActive = true;
                db.Accounts.Add(forumaccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(forumaccount);
        }

        //
        // GET: /ForumAccounts/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ForumAccount forumaccount = db.Accounts.Find(id);
            if (forumaccount == null)
            {
                return HttpNotFound();
            }
            return View(forumaccount);
        }

        //
        // POST: /ForumAccounts/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(ForumAccount forumaccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forumaccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(forumaccount);
        }

        //
        // GET: /ForumAccounts/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            ForumAccount forumaccount = db.Accounts.Find(id);
            if (forumaccount == null)
            {
                return HttpNotFound();
            }
            return View(forumaccount);
        }

        //
        // POST: /ForumAccounts/Delete/5

        [HttpPost, ActionName("Delete"), Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            ForumAccount forumaccount = db.Accounts.Find(id);
            db.Accounts.Remove(forumaccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}