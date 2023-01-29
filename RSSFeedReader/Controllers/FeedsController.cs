using Microsoft.AspNetCore.Mvc;
using RSSFeedReader.Data;
using RSSFeedReader.Models;
using RSSFeedReader.Services;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace RSSFeedReader.Controllers
{
    public class FeedsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IArticlesService _articlesService;

        public FeedsController(ApplicationDbContext context)
        {
            _articlesService = new ArticlesService();
            _context = context;
           
        }

        public IActionResult Index()
        {
            IEnumerable<Feed> feeds = _context.Feeds;
            return View(feeds);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feed feed)
        {
            var urlRegex = @"(http | http(s) ?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?";
            if (feed is null || feed.Url is null || !Regex.Match(feed.Url, urlRegex).Success) {
                ModelState.AddModelError("url", "Please enter a valid Url");
            }

            if (ModelState.IsValid)
            {
                IEnumerable<Article> articles = _articlesService.generateArticles(feed);
                feed.Articles = (List<Article>)articles;

                _context.Feeds.Add(feed);
                             
                foreach (Article article in articles)
                {
                    _context.Articles.Add(article);
                }

                _context.SaveChanges();

                Console.WriteLine(feed.Articles);
                Console.WriteLine("***********************");

                TempData["success"] = "Feed created successfully";

                return RedirectToAction("Index");
            }
            
            return View(feed);
        }

        public IActionResult Edit(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }

            var feed = _context.Feeds.Find(Id);
            //var feed = _context.Feeds.FirstOrDefault(x => x.Id == Id);
            //var feed = _context.Feeds.SingleOrDefault(x => x.Id == Id);

            if (feed == null)
            {
                return NotFound(nameof(feed));
            }

            return View(feed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Feed feed)
        {
            var urlRegex = @"(http | http(s) ?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?";
            if (feed is null || feed.Url is null || !Regex.Match(feed.Url, urlRegex).Success)
            {
                ModelState.AddModelError("url", "Please enter a valid Url");
            }

            if (ModelState.IsValid)
            {
                _context.Feeds.Update(feed);
                _context.SaveChanges();

                TempData["success"] = "Feed updated successfully";

                return RedirectToAction("Index");
            }

            return View(feed);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var feed = _context.Feeds.Find(Id);
            //var feed = _context.Feeds.FirstOrDefault(x => x.Id == Id);
            //var feed = _context.Feeds.SingleOrDefault(x => x.Id == Id);

            if (feed == null)
            {
                return NotFound(nameof(feed));
            }

            return View(feed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Feed feed)
        {
            if (feed != null)
            {
                _context.Feeds.Remove(feed);
                _context.SaveChanges();

                TempData["success"] = "Feed Deleted successfully";

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        [HttpPost]
        public IActionResult DeleteSelected(int[] selectedItems)
        {
           
            if (selectedItems != null)
            {
                var items = _context.Feeds.Where(i => selectedItems.Contains(i.Id)).ToList();
                _context.Feeds.RemoveRange(items);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            if(id == 0) 
            { 
                return NotFound() ;
            }

            var feed = _context.Feeds.Find(id);

            if(feed == null)
            {
                return NotFound();
            }

            IEnumerable<Article> articles = _context.Articles.Where(article => feed.Id == article.FeedId).ToList();

            feed.Articles = (List<Article>?)articles;

            return View(feed);
        }

        public IActionResult Reload(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var feed = _context.Feeds.Find(id);

            if(feed == null)
            {
                RedirectToAction("Details", new { id = id} );
            }

            var articles = _articlesService.generateArticles(feed);

            foreach(var article in articles)
            {
                if(_context.Articles.Where(a => a.Link == article.Link).ToList() == null)
                {
                    _context.Articles.Add(article);
                    feed.Articles.Add(article);
                }
            }

           _context.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }
    }

}
