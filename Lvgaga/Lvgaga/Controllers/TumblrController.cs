using System.Threading.Tasks;
using System.Web.Mvc;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Services;

namespace Lvgaga.Controllers
{
    public class TumblrController : Controller
    {
        private readonly ITumblrService _tumblrService;

        public TumblrController()
        {

        }

        public TumblrController(ITumblrService tumblrServic)
        {
            _tumblrService = tumblrServic;
        }

        // GET: Tumblr
        public async Task<ActionResult> Index()
        {
            return
                View(await _tumblrService.GetTumblrModelsAsync(LvConstants.PartitionKeyOfImage, TumblrCategory.All, 20));
        }
    }
}