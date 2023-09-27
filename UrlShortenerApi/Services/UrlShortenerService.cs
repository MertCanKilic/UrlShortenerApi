using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Interfaces;
using UrlShortenerApi.Types;

namespace UrlShortenerApi.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly AppDbContext _dbContext;
        public UrlShortenerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UrlShortenerResponse> AddShortUrlAsync(string longUrl)
        {
            if (Helper.isValidUrl(longUrl))
            {
                var selectedRow = await _dbContext.UrlTables
                                        .FirstOrDefaultAsync(s => s.LongUrl == longUrl) ?? new UrlTable();

                if (string.IsNullOrEmpty(selectedRow.ShortUrl))
                {
                    var code = Helper.GetRandomUrl();
                    var urlTable = new UrlTable
                    {
                        LongUrl = longUrl,
                        Code = code,
                        ShortUrl = $"{Constants.ShortSite}/{code}",
                        CreatedOnUtc = DateTime.Now
                    };

                    _dbContext.UrlTables.Add(urlTable);
                    await _dbContext.SaveChangesAsync();

                    var response = new UrlShortenerResponse
                    {
                        Code = 200,
                        Message = $"ShortUrl: {Constants.ShortSite}/{code} is added for Url: {longUrl}"
                    };
                    return response;
                }

                else
                {
                    var objectResponse = new UrlShortenerResponse{ 
                        Code = 400,
                        Message = $"Short Url for {longUrl} is already there({selectedRow.ShortUrl})"
                        };
                    return objectResponse;
                }
            }
            var badRequest = new UrlShortenerResponse
            {
                Code = 400,
                Message = $"Url: {longUrl} is not valid"
            };
            return badRequest;

        }

        public async Task<UrlShortenerResponse> AddShortUrlAsync(string longUrl, string code)
        {
            if (Helper.isValidUrl(longUrl))
            {
                var urlTable = await _dbContext.UrlTables
                    .FirstOrDefaultAsync(s => s.Code == code) ?? new UrlTable();
                string shortUrl = urlTable.ShortUrl ?? "";

                if (string.IsNullOrEmpty(shortUrl))
                {
                    urlTable.LongUrl = longUrl;
                    urlTable.ShortUrl = $"{Constants.ShortSite}/{code}";
                    urlTable.Code = code;
                    urlTable.CreatedOnUtc = DateTime.UtcNow;

                    _dbContext.UrlTables.Add(urlTable);
                    await _dbContext.SaveChangesAsync();

                    var response = new UrlShortenerResponse
                    {
                        Code = 200,
                        Message = $"ShortUrl: {Constants.ShortSite}/{code} is added for Url: {urlTable.LongUrl}"
                    };

                    return response;
                }

                else
                {
                    var response = new UrlShortenerResponse
                    {
                        Code = 202,
                        Message = $"Url: {Constants.ShortSite}/{code} is already there for Url: {urlTable.LongUrl} and cannot be used"
                    };

                    return response;
                }
            }

            var badRequest = new UrlShortenerResponse
            {
                Code = 400,
                Message = $"Url: {longUrl} is not valid"
            };

            return badRequest;
        }
    }
}
