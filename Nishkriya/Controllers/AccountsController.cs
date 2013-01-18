using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Controllers
{
    public class AccountsController : Controller
    {
        private ForumAccountDbContext db = new ForumAccountDbContext();
        
        //
        // GET: /Accounts/

        public ActionResult Index()
        {
            return View(db.ForumAccounts);
        }

        //
        // GET: /Accounts/id

        public ActionResult Details(int id)
        {
            ViewBag.id = id;
            return View(db.ForumAccounts);
        }
    }
}
