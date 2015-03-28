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

        // GET: Tumblr
        public async Task<ActionResult> Index()
        {
            return
                View(await _tumblrService.GetTumblrModelsAsync(LvConstants.PartitionKeyOfImage, TumblrCategory.All, 20));
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