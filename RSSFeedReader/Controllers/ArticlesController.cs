using Microsoft.AspNetCore.Mvc;
using RSSFeedReader.Data;
using RSSFeedReader.Models;

namespace RSSFeedReader.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Article> articles = _context.Articles;
            return View(articles);
        }

        [HttpPost]
        public IActionResult Index(DateTime from, DateTime to)
        {
            IEnumerable<Article> articles = _context.Articles.Where(x => x.PublishDate >= from && x.PublishDate <= to);
            return View(articles);
        }
    }
}
