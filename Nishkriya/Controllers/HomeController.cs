using System.Linq;
using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Controllers
{
    public class HomeController : Controller
    {
        NishkriyaContext db = new NishkriyaContext();

        public ActionResult Index()
        {
            ViewBag.accounts = db.Accounts.ToList();
            return View();
        }

    }
}
