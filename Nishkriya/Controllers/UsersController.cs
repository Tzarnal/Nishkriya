using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Nishkriya.Models;
using Nishkriya.Security;

namespace Nishkriya.Controllers
{
    public class UsersController : Controller
    {
        NishkriyaContext db = new NishkriyaContext();

        [HttpPost]
        public ActionResult Login(AuthenticationViewModel authenticationViewModel)
        {
            var user = db.Users.SingleOrDefault(a => a.Username == authenticationViewModel.Username);

            if (user != null && BCrypt.CheckPassword(authenticationViewModel.Password, user.Password))
            {
                var ticket = new FormsAuthenticationTicket(1, 
                                                           user.Username, 
                                                           DateTime.Now, 
                                                           DateTime.Now.AddMonths(1),
                                                           true, user.Username);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));

                Response.Cookies.Add(authCookie);
            }

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
