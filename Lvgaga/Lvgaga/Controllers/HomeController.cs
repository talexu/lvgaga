using System.Threading.Tasks;
using System.Web.Mvc;
using LvService.Services;

namespace Lvgaga.Controllers
{
    public class HomeController : Controller
    {
        private ITumblrService _tumblrService;

        public HomeController()
        {
            _tumblrService = new TumblrService();
        }

        public HomeController(ITumblrService tumblrService)
        {
            _tumblrService = tumblrService;
        }

        public async Task<ActionResult> Index()
        {
            //return View(await FakeDataHelper.GetFakeTumblrModelsAsync());
            return View(await _tumblrService.GetTumblrModelsAsync(20));
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