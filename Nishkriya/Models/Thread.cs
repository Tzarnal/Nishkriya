using Nishkriya.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace Nishkriya.Models
{
    public class Thread
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Title { get; set; }
        public virtual List<Post> Posts { get; set; }

        [NotMapped]
        public Uri Url
        {
            get
            {
                return
                    new Uri("http://forums.white-wolf.com/default.aspx?g=posts&t=" +
                            ThreadId.ToString(CultureInfo.InvariantCulture));
            }
        }

        public ThreadViewModel ToViewModel()
        {   
            return new ThreadViewModel
            {
                Id = this.Id,
                ThreadId = this.ThreadId,
                Title = this.Title,
                Url = this.Url
            };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Thread)) { return false; }
            var other = obj as Thread;
            return this.Id == other.Id && this.ThreadId == other.ThreadId;
        }
    }
}