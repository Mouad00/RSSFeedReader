using Microsoft.EntityFrameworkCore;
using RSSFeedReader.Models;

namespace RSSFeedReader.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
