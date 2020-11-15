using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StaticCacheHeader.Bsl.Articles.Cached;
using StaticCacheHeader.Models;

namespace StaticCacheHeader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleCached articleCached;
        public HomeController(ILogger<HomeController> logger, IArticleCached articleCached)
        {
            this._logger = logger;
            this.articleCached = articleCached;
        }

        public IActionResult Index()
        {
            var result = new List<ArticleModel>();
            var listArticle = articleCached.GetList();
            if (listArticle != null && listArticle.Any())
            {
                result = listArticle.Select(x => new ArticleModel(x)).ToList();
            }
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
