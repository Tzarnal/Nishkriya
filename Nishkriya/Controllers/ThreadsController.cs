using System.Web.Mvc;

namespace Nishkriya.Controllers
{
    public class ThreadsController : Controller
    {
        //
        // GET: /Threads/

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
