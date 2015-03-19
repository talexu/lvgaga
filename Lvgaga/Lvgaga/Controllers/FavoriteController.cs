using System.Web.Mvc;

namespace Lvgaga.Controllers
{
    [RoutePrefix("favorites")]
    public class FavoriteController : Controller
    {
        [Route]
        // GET: Favorite
        public ActionResult Index()
        {
            return View();
        }
    }
}