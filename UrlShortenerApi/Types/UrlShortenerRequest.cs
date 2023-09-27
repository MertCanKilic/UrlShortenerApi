

namespace UrlShortenerApi.Types
{
    public class UrlShortenerRequest
    {
        public string Code { get; set; } = String.Empty;
        public string LongUrl { get; set; } = String.Empty;
    }
}
