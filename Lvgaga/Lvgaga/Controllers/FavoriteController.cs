using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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