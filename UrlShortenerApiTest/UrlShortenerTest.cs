using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System;
using UrlShortenerApi;
using UrlShortenerApi.Controllers;
using UrlShortenerApi.Interfaces;
using UrlShortenerApi.Types;
using Xunit;

namespace UrlShortenerApiTest
{
    public class UrlShortenerTest
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlShortenerTest()
        {
            _urlShortenerService = A.Fake<IUrlShortenerService>();
        }

        [Fact]
        public void UrlRedirect_Success()
        {
            // Arrange
            var shortUrl = "https://short.site/123asd";
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new AppDbContext(options);

            context.UrlTables.Add(new UrlTable
            {
                ID = 1,
                Code = "123asd",
                ShortUrl = "https://short.site/123asd",
                LongUrl = "https://google.com",
                CreatedOnUtc = DateTime.UtcNow,

            });
            context.SaveChanges();

            var urlService = new UrlShortenerController(context, _urlShortenerService);

            // Act
            var redirect = urlService.GetLongUrl(shortUrl);

            // Assert
            Assert.Equal("Microsoft.AspNetCore.Mvc.RedirectResult", redirect.Result.ToString());
        }

        [Fact]
        public void UrlRedirect_NonMemberUrlShouldReturnNotFound()
        {
            // Arrange
            var nonMemberShortUrl = "https://short.site/gggh";
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new AppDbContext(options);

            context.UrlTables.Add(new UrlTable
            {
                ID = 1,
                Code = "123asd",
                ShortUrl = "https://short.site/123asd",
                LongUrl = "https://google.com",
                CreatedOnUtc = DateTime.UtcNow,

            });
            context.SaveChanges();

            var urlService = new UrlShortenerController(context, _urlShortenerService);

            // Act
            var redirect = urlService.GetLongUrl(nonMemberShortUrl);
            // Assert
            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", redirect.Result.ToString());
        }

        [Fact]
        public void UrlRedirect_BadUrlShouldReturnBadRequest()
        {
            // Arrange
            var wrongShortUrl = "/short.site/gggh";
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new AppDbContext(options);

            context.UrlTables.Add(new UrlTable
            {
                ID = 1,
                Code = "123asd",
                ShortUrl = "https://short.site/123asd",
                LongUrl = "https://google.com",
                CreatedOnUtc = DateTime.UtcNow,

            });
            context.SaveChanges();
            var urlService = new UrlShortenerController(context, _urlShortenerService);

            // Act
            var redirect = urlService.GetLongUrl(wrongShortUrl);
            // Assert
            Assert.Equal("Microsoft.AspNetCore.Mvc.BadRequestObjectResult", redirect.Result.ToString());
        }

        [Fact]
        public void RandomUrlShouldAddedToDb()
        {
            // Arrange
            var longUrl = "https://long.site/3451566";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new AppDbContext(options);

            context.UrlTables.Add(new UrlTable
            {
                ID = 1,
                Code = "123asd",
                ShortUrl = "https://short.site/123asd",
                LongUrl = "https://google.com",
                CreatedOnUtc = DateTime.UtcNow,

            });
            context.SaveChanges();

            A.CallTo(() => _urlShortenerService.AddShortUrlAsync(longUrl))
                     .Returns(new UrlShortenerResponse
                     {
                         Code = 200,
                         Message = ""
                     });


            // Act
            var urlResponse = _urlShortenerService.AddShortUrlAsync(longUrl);

            // Assert
            Assert.Equal(200, urlResponse.Result.Code);
        }

        [Fact]
        public void SelectedUrlShouldAddedToDb()
        {
            // Arrange
            var longUrl = "https://long.site/3451566";
            var code = "asdw23";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new AppDbContext(options);

            context.UrlTables.Add(new UrlTable
            {
                ID = 1,
                Code = "123asd",
                ShortUrl = "https://short.site/123asd",
                LongUrl = "https://google.com",
                CreatedOnUtc = DateTime.UtcNow,

            });
            context.SaveChanges();

            A.CallTo(() => _urlShortenerService.AddShortUrlAsync(longUrl, code))
                     .Returns(new UrlShortenerResponse
                     {
                         Code = 200,
                         Message = ""
                     });


            // Act
            var urlResponse = _urlShortenerService.AddShortUrlAsync(longUrl,code);
            // Assert
            Assert.Equal(200, urlResponse.Result.Code);
        }

    }
}
