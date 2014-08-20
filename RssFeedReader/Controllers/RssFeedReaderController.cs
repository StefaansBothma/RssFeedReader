using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace RssFeedReader.Controllers
{
    public class RssFeedReaderController : Controller
    {
        private readonly RssDBEntities _db = new RssDBEntities();

        public ActionResult Feed()
        {
            var items = new List<SyndicationItem>();

            var feed = SyndicationFeed.Load(XmlReader.Create("http://feeds.news24.com/articles/news24/TopStories/rss"));
            ViewBag.feedTitle = "No Title";

            if (feed != null)
            {
                ViewBag.feedTitle = feed.Title.Text;
                items.AddRange(
                    feed.Items.Select(item => item.Title.Text != null && item.Summary.Text != null && item.Links[0] != null ? new SyndicationItem(item.Title.Text, item.Summary.Text, item.Links[0].Uri) { PublishDate = item.PublishDate } : null));
                ViewBag.rssfeed = new RssFeed();
            }

            ViewBag.Feeds = items;
            return View();
        }


        //
        // GET: /Default1/

        public ActionResult Index()
        {
            return View(_db.RssFeeds.ToList());
        }

        //
        // GET: /Default1/Details/5

        public ActionResult Details(int id = 0)
        {
            RssFeed rssfeed = _db.RssFeeds.Find(id);
            if (rssfeed == null)
            {
                return HttpNotFound();
            }
            return View(rssfeed);
        }

        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Default1/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RssFeed rssfeed)
        {
            if (ModelState.IsValid)
            {
                _db.RssFeeds.Add(rssfeed);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rssfeed);
        }

        public void Test(RssFeed rssfeed)
        {

        }

        //
        // GET: /Default1/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RssFeed rssfeed = _db.RssFeeds.Find(id);
            if (rssfeed == null)
            {
                return HttpNotFound();
            }
            return View(rssfeed);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RssFeed rssfeed)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(rssfeed).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rssfeed);
        }

        //
        // GET: /Default1/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RssFeed rssfeed = _db.RssFeeds.Find(id);
            if (rssfeed == null)
            {
                return HttpNotFound();
            }
            return View(rssfeed);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RssFeed rssfeed = _db.RssFeeds.Find(id);
            _db.RssFeeds.Remove(rssfeed);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}