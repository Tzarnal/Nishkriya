using System.Web.Mvc;
using Nishkriya.Models;

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


        //
        // GET: /Threads/id

        public ActionResult Details(int id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
