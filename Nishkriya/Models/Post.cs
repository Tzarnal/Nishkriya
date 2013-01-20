using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nishkriya.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public string Hash { get; set; }
        public int ForumAccountId { get; set; }
        public virtual ForumAccount ForumAccount { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is Post) && (obj as Post).Hash.Equals(Hash);
        }

        public override int GetHashCode()
        {
            return (Hash != null ? Hash.GetHashCode() : 0);
        }
    }
}