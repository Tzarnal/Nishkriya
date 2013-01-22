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

        public void Scrape()
        {
            using (var db = new NishkriyaContext())
            {
                db.Accounts.ToList().ForEach(account => account.Posts.AddRange(GetNewPosts(account)));
                db.SaveChanges();
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

                var postsCollection = (from i in Enumerable.Range(1, 10)
                                       let tableRow = 1 + (2*i)
                                       
                                       let threadHref = document.DocumentNode.SelectSingleNode("id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab')//table//tr[" + tableRow + "]//td/a/@href").Attributes[0].Value
                                       let threadId = int.Parse(Regex.Match(threadHref, @"(\d+)$").Groups[0].Value)
                                       
                                       let threadTitle = document.DocumentNode.SelectSingleNode("id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab')//table//tr[" + tableRow + "]//td/a/text()").InnerHtml.Trim()
                                       
                                       let postDate = DateTime.Parse(document.DocumentNode.SelectSingleNode("id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab')//table//tr[" + tableRow + "]//td/text()[4]").InnerHtml.Trim())
                                       
                                       let postContent = document.DocumentNode.SelectSingleNode("id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_" + i + "')").InnerHtml
                                       select new Post
                                                  {
                                                      Content = postContent, 
                                                      Hash = _hashProvider.Compute(postContent), 
                                                      ThreadId = threadId, 
                                                      PostDate = postDate, 
                                                      PostTitle = threadTitle,
                                                  }).ToList();


                return postsCollection.Where(newPost => !account.Posts.Select(p => p.Hash).Contains(newPost.Hash));
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
