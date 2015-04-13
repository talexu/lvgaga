using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LvModel.Common;
using LvModel.View.Tumblr;
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

        [Route("{mediaType:int}")]
        public async Task<ActionResult> Index(string mediaType)
        {
            return await ActualIndex(mediaType);
        }

        // GET: Tumblr
        public async Task<ActionResult> Index()
        {
            return await ActualIndex(LvConstants.PartitionKeyOfAll);
        }

        private async Task<ActionResult> ActualIndex(string mediaType)
        {
            ViewBag.MediaType = mediaType;

            var homeModel = await _tumblrService.GetTumblrModelsAsync(mediaType, TumblrCategory.All, 20);
            if (homeModel == null || homeModel.Tumblrs == null || !homeModel.Tumblrs.Any()) return HttpNotFound();

            ViewBag.From = homeModel.Tumblrs.First().RowKey;
            ViewBag.To = homeModel.Tumblrs.Last().RowKey;

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