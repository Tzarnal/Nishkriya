using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using Nishkriya.Models;

namespace Nishkriya.Feeds
{
    public class FeedBuilder
    {
        readonly NishkriyaContext _db = new NishkriyaContext();

        public Rss20FeedFormatter Feed(string absoluteUri, UrlHelper url)
        {
            if (url == null) { throw new ArgumentNullException("url"); }

            var cssUri = new UriBuilder(absoluteUri)
            {
                // ReSharper disable Html.PathError - the bundler takes care of creating this path for us
                Path = url.Content("~/bundles/css")
                // ReSharper restore Html.PathError
            }.Uri;

            var items = _db.Posts
                        .OrderByDescending(p => p.PostDate)
                        .Take(20).ToList().Select(post => new SyndicationItem(
                        String.Format("{0} - {1} - {2}",
                            post.ForumAccount.Name,
                            post.Thread.Title,
                            post.Id),
                        String.Format("<link href='{0}' rel='stylesheet' /><br />{1}", cssUri, HttpUtility.HtmlDecode(post.Content)),
                        new UriBuilder(absoluteUri)
                        {
                            Path = url.Action("Details", "Posts", new { id = post.Id })
                        }.Uri))
                    .ToList();

            var feed = new SyndicationFeed("Nishkriya - Latest Posts", "An Exalted developer / writer tracker", new Uri(absoluteUri), items)
            {
                Language = "en-US",
                LastUpdatedTime = items.Max(i => i.PublishDate)
            };

            return new Rss20FeedFormatter(feed);
        }
    }
}