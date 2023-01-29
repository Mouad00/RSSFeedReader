using RSSFeedReader.Data;
using RSSFeedReader.Models;
using System.Xml.Linq;

namespace RSSFeedReader.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly ApplicationDbContext _context;

        public ArticlesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ArticlesService()
        {
        }

        private List<XElement> ReadRssFeed(string url)
        {
            var xml = XDocument.Load(url);

            if (xml.Root.Element("channel") != null)
            {
                var items = xml.Root.Element("channel").Elements("item").ToList();
                return items;
            }

            return null;
            
        }

        public IEnumerable<Article> generateArticles(Feed feed)
        {
            List<Article> articles = new List<Article>();

            if (feed is null)
            {
                return articles;
            }

            var FeedUrl = feed.Url;

            List<XElement> Items = ReadRssFeed(FeedUrl);

            if(Items == null)
            {
                return articles;
            }

            foreach (XElement item in Items)
            {
                Article article = new Article();

                article.Title = item.Element("title").Value;
                article.Link = item.Element("link").Value;
                article.Description = item.Element("description").Value;
                article.PublishDate = DateTime.Parse(item.Element("pubDate").Value);

               // article.Feed = feed;
               // article.FeedId = feed.Id;

                var media = item.Element("{http://search.yahoo.com/mrss/}thumbnail");
                if (media != null && media.Attribute("url") !=null)
                {
                    var url = media.Attribute("url").Value;
                    article.Image = url;
                }
                
                articles.Add(article);
            }
            return articles;
        }

    }
}
