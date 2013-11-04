using System;
using System.Collections.Generic;


namespace Nishkriya.Models.ViewModels
{
    public class NewContentViewModel
    {
        public List<ThreadViewModel> Threads { get; set; }
        public DateTime SessionTimeSinceLastVisit { get; set; }
        public bool HideExplanation;
        public bool NewContent;
    }
}