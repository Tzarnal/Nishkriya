using System.Data.Entity;


namespace Nishkriya.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ThreadId { get; set; }
        public string ThreadName { get; set; }
        public string Hash { get; set; }
    }

    public class PostDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
    }
}