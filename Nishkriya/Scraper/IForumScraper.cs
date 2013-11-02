using System.Collections.Generic;
using Nishkriya.Models;

namespace Nishkriya.Scraper
{
    public interface IForumScraper
    {
        void Scrape(NishkriyaContext db);
    }
}