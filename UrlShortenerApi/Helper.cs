using UrlShortenerApi.Types;

namespace UrlShortenerApi
{
    public class Helper
    {
        public static bool isValidUrl(string fullUrl)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(fullUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public static void ValidateUrl(string url)
        {
            if (!isValidUrl(url))
            {
                if(url == null || url == string.Empty)
                {

                }
            }
        }
        public static string GetRandomUrl()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string result = new string(
                Enumerable.Repeat(chars, Constants.MaxUrlLength)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}
