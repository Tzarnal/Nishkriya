using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Nishkriya.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nishkriya.Scraper
{
    public class VBulletinScavenger : IForumScraper
    {
        private readonly NishkriyaContext _db;
        private readonly IHashProvider _hashProvider;

        public VBulletinScavenger(IHashProvider hashProvider, NishkriyaContext db)
        {
            _db = db;
            _hashProvider = hashProvider;
        }

        public void Scrape()
        {

            //Don't scavange if there is not forumaccounts active on vb
            if (!_db.Accounts.Any(a => a.VbActive))
            {
                return;
            }

            var prospectiveThreads = GetActiveThreads();
            
            foreach (var thread in prospectiveThreads)
            {                
                ScrapeThread(thread);
            }

            foreach (var thread in prospectiveThreads)
            {
                var targetThread = _db.Threads.FirstOrDefault(a => a.ThreadId == thread.ThreadId);
                if (targetThread != null)
                {
                    foreach (var post in thread.Posts)
                    {
                        var targetPost = targetThread.Posts.FirstOrDefault(p => p.PostId == post.PostId);
                        if (targetPost != null)
                        {
                            if (targetPost.Hash != post.Hash)
                            {
                                targetPost.Hash = post.Hash;
                                targetPost.Content = post.Content;
                            }
                        }
                        else
                        {
                            targetThread.Posts.Add(post);
                        }
                    }
                }
                else
                {
                    _db.Threads.Add(thread);
                }
            }

            _db.SaveChanges();
        }

        private void ScrapeThread(Thread thread)
        {
            var names = _db.Accounts.Where(a => a.VbActive).Select(s => s.VbName).ToList();

            var url = thread.Url;
            var document = CleanHtml(UrlRequest(url.ToString()));

            if (document == null)
            {
                return;
            }

            var postXpathQuery = @"//li[contains(concat(' ', normalize-space(@class), ' '), ' b-post js-post')]";
            var accountQuery = @".//div[contains(concat(' ', normalize-space(@class), ' '), 'author ')]";
            var nextPageQuery = @"//a[contains(concat(' ', normalize-space(@class), ' '), ' arrow right-arrow ')]";

            do
            {
                var posts = document.DocumentNode.SelectNodes(postXpathQuery);

                foreach (var postNode in posts)
                {
                    var accountName = postNode.SelectSingleNode(accountQuery).InnerText.Trim();

                    if (names.Contains(accountName))
                    {
                        var post = PostParser(postNode);
                        thread.Posts.Add(post);
                    }
                }

                try
                {
                    var nextPageHref = document.DocumentNode.SelectSingleNode(nextPageQuery).Attributes["href"];
                    document = CleanHtml(UrlRequest(nextPageHref.Value));                    
                }
                catch (Exception)
                {
                    document = null;
                }

            } while (document != null);

        }

        private Post PostParser(HtmlNode postNode)
        {
            var post = new Post();

            var contentQuery = @".//div[contains(concat(' ', normalize-space(@class), ' '), ' js-post__content-text')]";
            post.Content = postNode.SelectSingleNode(contentQuery).InnerHtml.Trim();

            var accountQuery = @".//div[contains(concat(' ', normalize-space(@class), ' '), 'author ')]";
            var vbName = postNode.SelectSingleNode(accountQuery).InnerText.Trim();
            ForumAccount acc = _db.Accounts.Where(a => a.VbName == vbName).SingleOrDefault();
            post.ForumAccount = acc;
            
            var postIdQuery = @".//a[contains(concat(' ', normalize-space(@class), ' '), 'b-post__count')]";
            var postHref = postNode.SelectSingleNode(postIdQuery).Attributes["href"].Value;

            var matchedNumbers = Regex.Matches(postHref, @"\d+");
            var postId = matchedNumbers[matchedNumbers.Count - 1].Value;
            post.PostId = int.Parse(postId);

            var timeString = postNode.SelectSingleNode(@".//time").Attributes["datetime"].Value.Trim();
            post.PostDate = DateTime.Parse(timeString);

            post.Hash = _hashProvider.Compute(post.Content);

            return post;
        }

        private List<Thread> GetActiveThreads()
        {
            var pageCount = 4; // 60 threads should basically be enough.
            var activeThreads = new List<Thread>();

            var authorNames = "[%22" + string.Join("%22%2C%22", _db.Accounts.Where(a => a.VbActive).Select(s => s.VbName).ToList()) + "%22]";
            var nextPageQuery = @"//a[contains(concat(' ', normalize-space(@class), ' '), ' arrow right-arrow ')]";

            //var url = @"http://forum.theonyxpath.com/search?searchJSON={%22author%22%3A" + authorNames + @"%2C%22sort%22%3A{%22relevance%22%3A%22desc%22}%2C%22view%22%3A%22topic%22%2C%22exclude_type%22%3A[%22vBForum_PrivateMessage%22]}";
            var url = @"http://forum.theonyxpath.com/search?searchJSON={%22author%22%3A"+ authorNames +"%2C%22channel%22%3A[%2222%22]%2C%22sort%22%3A{%22lastcontent%22%3A%22desc%22}%2C%22view%22%3A%22topic%22%2C%22exclude_type%22%3A[%22vBForum_PrivateMessage%22]}";
        
            var document = UrlRequest(url);
           
            do
            {
                pageCount--;
                var topics = document.DocumentNode.SelectNodes(@"//a[@class='topic-title']");
    
                if (topics != null)
                {
                    foreach (var topic in topics)
                    {
                        var threadUrl = topic.Attributes["href"].Value;
                        var threadName = topic.InnerText;

                        var regexResult = Regex.Match(threadUrl, @"\d+");
                        var threadId = int.Parse(regexResult.Groups[0].Value);

                        if (!activeThreads.Any(a => a.ThreadId == threadId))
                        {
                            activeThreads.Add(new Thread
                            {
                                ThreadId = threadId,
                                Title = threadName,
                                Posts = new List<Post>(),
                                Type = 2
                            });    
                        }                        
                    }
                }

                try
                {
                    var nextPageHref = document.DocumentNode.SelectSingleNode(nextPageQuery).Attributes["href"];
                    var nexPageUrl = nextPageHref.Value.Replace("amp;", string.Empty);
                    document = CleanHtml(UrlRequest(nexPageUrl));
                }
                catch (Exception)
                {
                    document = null;
                }

            } while (document != null && pageCount > 0);


            
            return activeThreads;
        }


        private HtmlDocument CleanHtml(HtmlDocument document)
        {
            if (document == null)
            {
                return null;
            }
            
            //Remove Spoiler Buttons
            var spoilersButtons = document.DocumentNode.SelectNodes("//input[@type='button']");
            if (spoilersButtons != null)
            {
                foreach (var node in spoilersButtons)
                {
                    node.ParentNode.ChildNodes.Remove(node);
                }
            }

            //Set spoilers to actually have a class and id
            var spoilers = document.DocumentNode.SelectNodes("//div[@style='display: none;']");
            if (spoilers != null)
            {
                foreach (var node in spoilers)
                {
                    node.Id = "spoilerId";
                    node.SetAttributeValue("class", "spoilerbox");
                    node.Attributes.Remove("Style");
                }
            }


            //Strip inline Styles
            //Set spoilers to actually have a class and id
            var styleElements = document.DocumentNode.SelectNodes("//*[@style]");
            if (styleElements != null)
            {
                foreach (var node in styleElements)
                {
                    node.SetAttributeValue("style", "");
                }
            }

            //Add quote classes to quote blocks
            var quoteBlockElement = document.DocumentNode.SelectNodes("//div[@class='quote_container']");
            if (quoteBlockElement != null)
            {
                foreach (var node in quoteBlockElement)
                {
                    node.SetAttributeValue("class", "quote");
                }
            }

            //Add quote classes to quote blocks
            var quoteBlockInnerElement = document.DocumentNode.SelectNodes("//div[@class='message']");
            if (quoteBlockInnerElement != null)
            {
                foreach (var node in quoteBlockInnerElement)
                {
                    node.SetAttributeValue("class", "innerquote");
                }
            }

            return document;
        }

        private HtmlDocument UrlRequest(string url)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";

            Stream responseStream;

            try
            {
                responseStream = req.GetResponse().GetResponseStream();                
            }
            catch (WebException)
            {
                return null;
            }
            
                        
            var document = new HtmlDocument();

            if (responseStream == null)
            {
                return null;
            }

            using (var reader = new StreamReader(responseStream))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream))
                    {
                        writer.Write(reader.ReadToEnd());
                        memoryStream.Position = 0;
                        document.Load(memoryStream, new UTF8Encoding());
                    }
                }
            }

            return document;
        }

    }
}