using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nishkriya.Models.ViewModels
{
    public class ThreadViewModel
    {
        public string Title { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public Uri Url { get; set; }
    }
}