using System.Web.Mvc;

namespace Nishkriya.Controllers
{
    public class AccountsController : Controller
    {
        //
        // GET: /Accounts/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Details(int id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
