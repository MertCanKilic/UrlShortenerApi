using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Types;

namespace UrlShortenerApi.Interfaces
{
    public interface IUrlShortenerService
    {
        public Task<UrlShortenerResponse> AddShortUrlAsync(string longUrl);
        public Task<UrlShortenerResponse> AddShortUrlAsync(string longUrl, string code);
    }
}
