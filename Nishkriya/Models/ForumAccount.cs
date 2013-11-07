using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nishkriya.Models
{
    public class ForumAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string VbName { get; set; }
        public string Name { get; set; }
        public bool YafActive { get; set; }
        public bool VbActive { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}