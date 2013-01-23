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
                db.Accounts.Where(a => a.Active).ToList().ForEach(account => account.Posts.AddRange(GetNewPosts(account, db.Threads.ToList())));
                db.SaveChanges();
            }
        }

        private IEnumerable<Post> GetNewPosts(ForumAccount account, List<Thread> threads)
        {
            try
            {
                var url = string.Format(Settings.Default.ProfileUrl, account.ForumId);

                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(new Cookie(".YAFNET_Authentication", Settings.Default.AuthToken, "/", "forums.white-wolf.com"));

                var response = (HttpWebResponse)req.GetResponse();
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

                var postsCollection = new List<Post>();

                const string placeholderFragment = "id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab')//table//tr[";
                const string anchorSelectorFragment = "]//td/a/@href";
                const string titleSelectorFragment = "]//td/a/text()";
                const string dateSelectorFragment = "]//td/text()[4]";

                foreach (var node in document.DocumentNode.ChildNodes.Where(child => child.Attributes.Any(a => a.Name == "class" && a.Value == "spoilertitle")))
                {
                    document.DocumentNode.RemoveChild(node);
                }

                foreach (var node in document.DocumentNode.ChildNodes.Where(child => child.Attributes.Any(a => a.Name == "class" && a.Value == "spoilerbox")))
                {
                    document.DocumentNode.RemoveChild(node);
                }

                foreach (int i in Enumerable.Range(0, 10))
                {
                    var tableRow = 1 + (2 * i);

                    var threadIdSelector = String.Format("{0}{1}{2}", placeholderFragment, tableRow, anchorSelectorFragment);
                    var titleSelector = String.Format("{0}{1}{2}", placeholderFragment, tableRow, titleSelectorFragment);
                    var dateSelector = String.Format("{0}{1}{2}", placeholderFragment, tableRow, dateSelectorFragment);

                    var threadHref = document.DocumentNode.SelectSingleNode(threadIdSelector).Attributes[0].Value;
                    var threadId = int.Parse(Regex.Match(threadHref, @"(\d+)$").Groups[0].Value);

                    var threadTitle = document.DocumentNode.SelectSingleNode(titleSelector).InnerHtml.Trim();

                    var postDate = DateTime.Parse(document.DocumentNode.SelectSingleNode(dateSelector).InnerHtml.Trim());

                    var postContent =
                        document.DocumentNode.SelectSingleNode(
                            "id('MasterPageContentPlaceHolder_forum_ctl01_ProfileTabs_Last10PostsTab_LastPosts_MessagePost_" + i + "')").InnerHtml;

                    Thread thread = threads.SingleOrDefault(s => s.ThreadId == threadId);
                    if (thread == null)
                    {
                        thread = new Thread { ThreadId = threadId, Title = threadTitle };
                        threads.Add(thread);
                    }

                    postsCollection.Add(new Post
                    {
                        Content = postContent,
                        Hash = _hashProvider.Compute(postContent),
                        PostDate = postDate,
                        Thread = thread
                    });
                }


                return postsCollection.Where(newPost => !account.Posts.Select(p => p.Hash).Contains(newPost.Hash));
            }
            catch (Exception ex)
            {
                using (var db = new NishkriyaContext())
                {
                    db.Errors.Add(new Error { StackTrace = ex.StackTrace, OccurredOn = DateTime.Now });
                    Debug.WriteLine(ex.Message + "\n----\n" + ex.StackTrace);
                }
                return new Post[0];
            }
        }
    }
}
