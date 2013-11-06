using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nishkriya.Models.ViewModels
{
    public class ThreadViewModel
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Title { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public Uri Url { get; set; }
    }
}