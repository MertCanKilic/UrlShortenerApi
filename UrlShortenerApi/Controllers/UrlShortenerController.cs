using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Interfaces;
using UrlShortenerApi.Types;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlShortenerController(AppDbContext dbContext, IUrlShortenerService urlShortenerService)
        {
            _dbContext = dbContext;
            _urlShortenerService = urlShortenerService;
        }

        [HttpGet()]
        public async Task<ActionResult> GetLongUrl(string shortUrl)
        {
            string shortUrlDecoded = System.Web.HttpUtility.UrlDecode(shortUrl);
            if (!Helper.isValidUrl(shortUrlDecoded))
            {
                return BadRequest($"{shortUrlDecoded} is not a valid Url");
            }

            var split = shortUrlDecoded.Split('/');
            var code = split[split.Length-1];

            var shortenedUrl = await _dbContext.UrlTables
                .FirstOrDefaultAsync(s => s.Code == code);

            if (shortenedUrl is null)
            {
                return NotFound();
            }

            return Redirect(shortenedUrl.LongUrl);
        }

        [HttpPost()]
        public async Task<ActionResult<List<UrlTable>>> AddShortUrl(UrlShortenerRequest request)
        {
            var action = new UrlShortenerResponse();
            if (string.IsNullOrEmpty(request.Code))
               action = await _urlShortenerService.AddShortUrlAsync(request.LongUrl);
            else
               action = await _urlShortenerService.AddShortUrlAsync(request.LongUrl, request.Code);

            return StatusCode(action.Code,action.Message);
        }
    }
}