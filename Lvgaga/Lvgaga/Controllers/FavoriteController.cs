using System.Threading.Tasks;
using System.Web.Mvc;
using LvModel.Common;
using LvModel.View.Favorite;
using LvService.Services;
using Microsoft.AspNet.Identity;

namespace Lvgaga.Controllers
{
    [RoutePrefix("favorites")]
    public class FavoriteController : Controller
    {
        private readonly ISasService _sasService;

        public FavoriteController()
        {

        }

        public FavoriteController(ISasService sasService)
        {
            _sasService = sasService;
        }

        // GET: Favorite
        [Route]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(new FavoritesModel
            {
                Sas = await _sasService.GetSasForTable(LvConstants.TableNameOfFavorite, User.Identity.GetUserId())
            });
        }
    }
}