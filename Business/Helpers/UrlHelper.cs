using System;
using System.Linq;

namespace Business.Helpers
{
    public static class UrlHelper
    {
        public static string GenerateShortUrl(string longUrl)
        {
            Uri url = new Uri(longUrl);
            string host = url.GetLeftPart(UriPartial.Authority);

            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return $"{host.Replace("-", ".")}/{result}/";
        }
    }
}
