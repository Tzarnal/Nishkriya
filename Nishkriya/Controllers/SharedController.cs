using System.Linq;
using System.Web.Mvc;
using Nishkriya.Models;
using Nishkriya.Models.ViewModels;

namespace Nishkriya.Controllers
{
    public class SharedController : Controller
    {
        NishkriyaContext db = new NishkriyaContext();

        public ActionResult _SidebarHome(dynamic selected)
        {
            ViewBag.selectedSidebarEntry = selected;
            return PartialView(new SidebarViewModel { selectedSidebarEntry = selected, accountList = db.Accounts.ToList() });
        }

        public ActionResult _SidebarPosters(dynamic selected)
        {
            ViewBag.selectedSidebarEntry = selected;
            return PartialView(new SidebarViewModel { selectedSidebarEntry = selected, accountList = db.Accounts.ToList() });
        }

        public ActionResult Analytics()
        {
            if (Properties.Settings.Default.EnableAnalytics && !string.IsNullOrWhiteSpace(Properties.Settings.Default.GACode))
            {
                return PartialView("_Analytics", Properties.Settings.Default.GACode);
            }
            return new EmptyResult();
        }
    }
}
