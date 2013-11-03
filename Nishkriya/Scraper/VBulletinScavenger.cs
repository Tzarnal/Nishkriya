using Nishkriya.Models;
using System.Collections.Generic;
using System.Linq;

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

        public List<ForumAccount> GetAccounts() 
        {
            return _db.Accounts.Where(a => a.Active).ToList(); ; // add check for new forum Id later
        }

        private class ProspectivePost
        {
            public ForumAccount Account {get;set;}
            public int ExternalId {get;set;}
            public string Body {get;set;}
            //public BulletinThread Thread {get;set;}
        }

    }
}