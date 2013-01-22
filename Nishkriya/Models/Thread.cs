using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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

        public Uri GetUrl()
        {
            return new Uri("http://forums.white-wolf.com/default.aspx?g=posts&t=" + ThreadId.ToString(CultureInfo.InvariantCulture));
        }
    }
}