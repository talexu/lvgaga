using System.Web.Mvc;
using LvService.Services;

namespace Lvgaga.Controllers
{
    [RoutePrefix("favorites")]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController()
        {

        }

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // GET: Favorite
        [Route]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}