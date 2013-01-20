using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Controllers
{
    public class ForumAccountsController : Controller
    {
        private readonly NishkriyaContext db = new NishkriyaContext();

        //
        // GET: /ForumAccounts/

        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        //
        // GET: /ForumAccounts/Details/5

        public ActionResult Details(int id = 0)
        {
            ForumAccount forumaccount = db.Accounts.Find(id);
            if (forumaccount == null)
            {
                return HttpNotFound();
            }
            return View(forumaccount);
        }

        //public ActionResult Details(string name)
        //{
        //    if (name == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var account = db.Accounts.SingleOrDefault(a => a.Name.ToUpper() == name.ToUpper());

        //    if (account == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(account);
        //}

        //
        // GET: /ForumAccounts/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ForumAccounts/Create

        [HttpPost]
        public ActionResult Create(ForumAccount forumaccount)
        {
            if (ModelState.IsValid)
            {
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

        [HttpPost, ActionName("Delete")]
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