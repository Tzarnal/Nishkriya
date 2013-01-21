using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;
using Nishkriya.Models;
using Nishkriya.Properties;

namespace Nishkriya.Scraper
{
    public class ScavengerLord
    {
        private readonly IHashProvider _hashProvider;

        public ScavengerLord(IHashProvider hashProvider)
        {
            _hashProvider = hashProvider;
        }

        static readonly List<String> PostIds = new List<string>
            {
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_0",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_1",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_2",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_3",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_4",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_5",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_6",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_7",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_8",
                "MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_9"
            };

        public void Scrape()
        {
            using (var db = new NishkriyaContext())
            {
                db.Accounts.ToList().ForEach(account => account.Posts.AddRange(GetNewPosts(account)));
                //db.SaveChanges();
            }
        }

        private IEnumerable<Post> GetNewPosts(ForumAccount account)
        {
            try
            {
                var url = string.Format(Settings.Default.ProfileUrl, account.ForumId);

                var req = (HttpWebRequest) WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(new Cookie(".YAFNET_Authentication", Settings.Default.AuthToken, "/", "forums.white-wolf.com"));

                var response = (HttpWebResponse) req.GetResponse();
                var document = new HtmlDocument();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var writer = new StreamWriter(memoryStream))
                        {
                            writer.Write(reader.ReadToEnd());
                            memoryStream.Position = 0;
                            document.Load(memoryStream);
                        }
                    }
                }



                var threadHref =
                    document.DocumentNode.SelectSingleNode(
                        "id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab')//table//tr[1]//td/a/@href").Attributes[0].Value;
                var threadId = int.Parse( Regex.Match(threadHref, @"(\d+)$").Groups[0].Value );

                var threadTitle =
                    document.DocumentNode.SelectSingleNode(
                        "id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab')//table//tr[1]//td/a/text()").InnerHtml.Trim();

                var postDate = DateTime.Parse( 
                    document.DocumentNode.SelectSingleNode(
                        "id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab')//table//tr[1]//td/text()[4]").InnerHtml.Trim());

                
                var postContent =
                    document.DocumentNode.SelectSingleNode(
                        "id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_" + 1 + "')" ).InnerHtml;




                return null;
            }
            catch (Exception ex)
            {
                using (var db = new NishkriyaContext())
                {
                    db.Errors.Add(new Error {StackTrace = ex.StackTrace, OccurredOn = DateTime.Now});
                    Debug.WriteLine(ex.Message + "\n----\n" + ex.StackTrace);
                }
                return new Post[0];
            }
        }
    }
}