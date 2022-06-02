using System;
using System.Linq;

namespace Business.Helpers
{
    public static class UrlHelper
    {
        public static string GenerateShortUrl(string longUrl)
        {
            string host = new Uri(longUrl).Host;

            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return host + "/" + result;
        }
    }
}
