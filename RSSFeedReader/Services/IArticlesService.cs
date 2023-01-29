using RSSFeedReader.Data;
using RSSFeedReader.Models;

namespace RSSFeedReader.Services
{
    public interface IArticlesService
    {
        public IEnumerable<Article> generateArticles(Feed feed);   
    }
}
