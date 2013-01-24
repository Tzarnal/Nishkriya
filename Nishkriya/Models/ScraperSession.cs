using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nishkriya.Models
{
    public class ScraperSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public int PostsAdded { get; set; }
        public int ThreadsAdded { get; set; }

        [NotMapped]
        public TimeSpan TimeTaken
        {
            get { return Finish - Start; }
        }
    }
}