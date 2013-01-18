using System.Data.Entity;


namespace Nishkriya.Models
{
    public class Thread
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Name { get; set; }
        public int FirstPost { get; set; }
        
    }

    public class ThreadDbContext : DbContext
    {
        public DbSet<Thread> Threads { get; set; }
    }
}