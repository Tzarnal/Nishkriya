using System.Data.Entity;

namespace Nishkriya.Models
{
    public class ForumAccount
    {

        public int Id { get; set; }
        public int ForumId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

    }

    public class ForumAccountDbContext : DbContext
    {
        public DbSet<ForumAccount> ForumAccounts { get; set; }
    }

}