﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nishkriya.Models;

namespace Nishkriya.Scraper
{
    public class ScavengerLord
    {
        public void Run()
        {
            using (var db = new NishkriyaContext())
            {
                var hasher = new Sha1Provider();
                var scrapers = new List<IForumScraper> {new YAFScavenger(hasher, db), new VBulletinScavenger(hasher,db)};
                scrapers.ForEach(s => s.Scrape());
            }
        }
    }
}