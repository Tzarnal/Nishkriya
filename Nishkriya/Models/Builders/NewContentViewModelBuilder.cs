using System;
using System.Linq;
using Nishkriya.Models.ViewModels;
using Nishkriya.Extensions;

namespace Nishkriya.Models.Builders
{
    public class NewContentViewModelBuilder
    {
        private NishkriyaContext _db;

        public NewContentViewModelBuilder(NishkriyaContext db)
        {
            _db = db;
        }

        public NewContentViewModel Build(bool hideExplanation, DateTime sessionTimeSinceLastVisit)
        {
            var newContent = new NewContentViewModel();

            var threads =
                _db.Threads.Where(thread => thread.Posts.Max(post => post.PostDate) > sessionTimeSinceLastVisit)
                   .ToViewModels(true).ToList();
                                     
            
            newContent.SessionTimeSinceLastVisit = sessionTimeSinceLastVisit;            
            newContent.HideExplanation = hideExplanation;
            newContent.Threads = threads;
            newContent.NewContent = threads.Any();

            return newContent;
        }
    }
}