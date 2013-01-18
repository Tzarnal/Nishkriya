using System.Data.Entity;


namespace Nishkriya.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? FoumAccount { get; set; } // Can be null
        public string PosterName { get; set; } 
    }

    public class PostDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
    }
}