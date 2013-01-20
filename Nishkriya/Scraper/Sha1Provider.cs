using System;
using System.Security.Cryptography;
using System.Text;

namespace Nishkriya.Scraper
{
    public class Sha1Provider : IHashProvider
    {
        public string Compute(string input)
        {
            return BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
        }
    }
}