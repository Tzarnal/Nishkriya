using System;
using Nishkriya.Extensions;

namespace Nishkriya.Models.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public ForumAccount Account { get; set; }
        public string Content { get; set; }
        public ThreadViewModel Thread { get; set; }
        public int ThreadId { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public Uri Url { get; set; }

        public override string ToString()
        {
            return Content;
        }

        public string TimeSincePost()
        {
            return PostDate.TimeSince();
        }
    }
}