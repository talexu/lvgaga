using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LvModel.Common;
using LvService.Services;
using Microsoft.AspNet.Identity;

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