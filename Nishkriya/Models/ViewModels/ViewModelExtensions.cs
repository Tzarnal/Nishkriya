using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nishkriya.Models.ViewModels
{
    public static class ViewModelExtensions
    {
        public static ICollection<PostViewModel> ToViewModels(this IEnumerable<Post> source) 
        {
            var threads = source.Select(s => s.Thread).Distinct().ToViewModels().ToList();
            var postViewModels = new List<PostViewModel>();

            foreach (var thread in threads)
            {
                var toAdd = source.Where(s => s.Thread.ThreadId == thread.ThreadId)
                                  .Select(s => s.ToViewModel(thread))
                                  .ToList();
                thread.Posts = toAdd;
                postViewModels.AddRange(toAdd);
            }

            return postViewModels;
        }

        public static ICollection<ThreadViewModel> ToViewModels(this IEnumerable<Thread> source, bool createPosts = false)
        {
            if (createPosts)
            {
                var viewModels = new List<ThreadViewModel>();
                foreach (var thread in source)
                {
                    var threadViewModel = thread.ToViewModel();
                    threadViewModel.Posts = thread.Posts.Select(p => p.ToViewModel(threadViewModel)).ToList();
                    viewModels.Add(threadViewModel);
                }
                return viewModels;
            }
            else
            {
                return source.Select(s => s.ToViewModel()).ToList();
            }
        }
    }
}