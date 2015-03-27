using System.Threading.Tasks;
using System.Web.Mvc;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Tumblr;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Factories.ViewModel;
using LvService.Services;
using Microsoft.WindowsAzure.Storage;

namespace Lvgaga.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITumblrService _tumblrService;

        public HomeController()
        {
            _tumblrService = new TumblrService(
                new AzureStoragePool(new AzureStorageDb()),
                new ReadTableEntitiesCommand(new ReadTumblrEntityWithCategoryCommand()),
                new TumblrFactory(new UriFactory()));
        }

        public HomeController(ITumblrService tumblrService)
        {
            _tumblrService = tumblrService;
        }

        public async Task<ActionResult> Index()
        {
            //return View(await FakeDataHelper.GetFakeTumblrModelsAsync());
            return View(await _tumblrService.GetTumblrModelsAsync(LvConstants.PartitionKeyOfImage, TumblrCategory.All, 20));
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