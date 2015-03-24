using System.Threading.Tasks;
using System.Web.Mvc;
using LvService.Utilities;

namespace Lvgaga.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View(await FakeDataHelper.GetFakeTumblrModelsAsync());
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