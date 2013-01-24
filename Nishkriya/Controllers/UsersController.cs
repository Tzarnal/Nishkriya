using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Nishkriya.Models;
using Nishkriya.Models.ViewModels;
using Nishkriya.Properties;
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
                Response.Cookies.Add(GenerateAuthenticationCookie(user));
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewUserViewModel newUser)
        {
            if (ModelState.IsValid && newUser.Secret == Settings.Default.SecretAnswer)
            {
                var user = new User
                    {
                        Username = newUser.Username,
                        Password = BCrypt.HashPassword(newUser.Password, BCrypt.GenerateSalt()),
                        Email = newUser.Email,
                        IsAdmin = true,
                        CreatedAt = DateTime.Now
                    };

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(newUser);
        }

        private HttpCookie GenerateAuthenticationCookie(User user)
        {
            var ticket = new FormsAuthenticationTicket(1,
                                                       user.Username,
                                                       DateTime.Now,
                                                       DateTime.Now.AddMonths(1),
                                                       true, user.Username);

            return new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
        }

    }
}
