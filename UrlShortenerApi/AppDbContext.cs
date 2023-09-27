using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Types;

namespace UrlShortenerApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public AppDbContext()
        {

        }
        public DbSet<UrlTable> UrlTables => Set<UrlTable>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlTable>(builder =>
            {
                builder.Property(s => s.ShortUrl).HasMaxLength(Constants.MaxUrlLength);
                builder.HasIndex(s => s.ShortUrl).IsUnique();
            });
        }
    }
}
