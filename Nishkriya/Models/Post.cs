namespace Nishkriya.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Hash { get; set; }
        public int AuthorId { get; set; }
        public virtual ForumAccount Author { get; set; }

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