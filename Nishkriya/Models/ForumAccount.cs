using System.Collections.Generic;

namespace Nishkriya.Models
{
    public class ForumAccount
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}