using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nishkriya.Models.ViewModels
{
    public class SidebarViewModel
    {
        public string selectedSidebarEntry { get; set; }
        public IList<ForumAccount> accountList { get; set; }

    }
}