using System.Data.Entity;

namespace Nishkriya.Models
{
    public class NishkriyaContext : DbContext
    {
        public DbSet<ForumAccount> Accounts { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}