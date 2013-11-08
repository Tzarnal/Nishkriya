using Nishkriya.Extensions;
using Nishkriya.Models.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nishkriya.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? PostId { get; set; }
        public string Content { get; set; }
        public string Hash { get; set; }
        public virtual ForumAccount ForumAccount { get; set; }
        public virtual Thread Thread { get; set; }
        public DateTime PostDate { get; set; }
        
        public override bool Equals(object obj)
        {
            return (obj is Post) && (obj as Post).Hash.Equals(Hash);
        }

        public override int GetHashCode()
        {
            return (Hash != null ? Hash.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Content;
        }

        public string TimeSincePost()
        {
            return PostDate.TimeSince();
        }


        public PostViewModel ToViewModel(ThreadViewModel thread)
        {
            return new PostViewModel
            {
                Id = this.Id,
                Account = this.ForumAccount,
                Content = this.Content,
                PostDate = this.PostDate,
                Thread = thread,
                Url = this.PostId.HasValue ? new Uri(thread.Url + "/"+  PostId.Value) : thread.Url,
            };
        }
    }
}