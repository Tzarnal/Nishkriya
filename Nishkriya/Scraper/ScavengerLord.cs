using System.IO;
using System.Net;

namespace Nishkriya.Scraper
{
    public class ScavengerLord
    {
        public void Scrape()
        {
            var loginUrl = @"http://www.white-wolf.com/";
            var req = (HttpWebRequest) WebRequest.Create(loginUrl);
            req.Timeout = 60 * 1000;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            var postData = "ctl00$login$loginView$aspLogin$UserName=Nishkriyabot&ctl00$login$loginView$aspLogin$Password=i am not a bot";

            req.ContentLength = postData.Length;

            var outWriter = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            outWriter.Write(postData);
            outWriter.Close();

            var response = req.GetResponse();

       }
    }
}