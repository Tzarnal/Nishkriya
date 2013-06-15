using System;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Nishkriya.Feeds
{
    public class FeedResult : ActionResult
    {
        public Encoding Encoding { get; set; }
        public string ContentType { get; set; }
        public SyndicationFeedFormatter Feed { get; private set; }

        public FeedResult(SyndicationFeedFormatter feed)
        {
            if (feed == null)
            {
                throw new ArgumentNullException("feed");
            }
            Feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = ContentType ?? "application/rss+xml";

            if (Encoding != null)
            {
                response.ContentEncoding = Encoding;
            }

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                Feed.WriteTo(writer);
            }
        }
    }
}