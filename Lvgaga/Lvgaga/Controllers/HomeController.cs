﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Common;
using LvService.Services;

namespace Lvgaga.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITumblrService _tumblrService;

        public HomeController()
        {

        }

        public HomeController(ITumblrService tumblrServic)
        {
            _tumblrService = tumblrServic;
        }

        [Route("medias/{mediaType:int}")]
        public async Task<ActionResult> Index(int mediaType)
        {
            return await ActualIndex(mediaType, (int)TumblrCategory.All);
        }

        [Route("medias/{mediaType:int}/categories/{category:int}")]
        public async Task<ActionResult> Index(int mediaType, int category)
        {
            return await ActualIndex(mediaType, category);
        }

        // GET: Tumblr
        //[OutputCache(CacheProfile = "OutputCacheProfile")]
        public async Task<ActionResult> Index()
        {
            return await ActualIndex((int)MediaType.All, (int)TumblrCategory.All);
        }

        private async Task<ActionResult> ActualIndex(int mediaType, int category)
        {
            var homeModel =
                await
                    _tumblrService.GetTumblrsModelAsync(
                        mediaType.ToString(),
                        (TumblrCategory)category,
                        LvConfiguration.TakingCount);
            if (homeModel == null || homeModel.Tumblrs == null || !homeModel.Tumblrs.Any()) return HttpNotFound();

            homeModel.MediaType = mediaType;
            homeModel.TumblrCategory = category;
            return View(homeModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}