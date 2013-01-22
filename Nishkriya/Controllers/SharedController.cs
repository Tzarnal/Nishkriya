using System.Linq;
using System.Web.Mvc;
using Nishkriya.Models;
using Nishkriya.Models.ViewModels;

namespace Nishkriya.Controllers
{
    public class SharedController : Controller
    {

        NishkriyaContext db = new NishkriyaContext();

        public ActionResult _Sidebar(dynamic selected)
        {
            ViewBag.selectedSidebarEntry = selected;
            return PartialView(new SidebarViewModel{selectedSidebarEntry = selected, accountList = db.Accounts.ToList()});
        }

    }
}
