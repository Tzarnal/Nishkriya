using Nishkriya.Models;

namespace Nishkriya.Scraper
{
    public class VBulletinScavenger : IForumScraper
    {
        private readonly NishkriyaContext _db;

        public VBulletinScavenger(NishkriyaContext db)
        {
            _db = db;
        }

        public void Scrape()
        {
        }
    }
}