using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nishkriya.Controllers
{
    public class PostsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Page(int id = 0)
        {
            return View();

        }

        public ActionResult LatestTopics()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
