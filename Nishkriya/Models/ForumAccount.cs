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
        public string Name { get; set; }
        public bool Active { get; set; }
        public virtual List<YafPost> Posts { get; set; }
    }
}