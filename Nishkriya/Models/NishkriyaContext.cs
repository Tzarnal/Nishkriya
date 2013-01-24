using System.Data.Entity;

namespace Nishkriya.Models
{
    public class NishkriyaContext : DbContext
    {
        public DbSet<ForumAccount> Accounts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<ScraperSession> Stats { get; set; }
    }
}