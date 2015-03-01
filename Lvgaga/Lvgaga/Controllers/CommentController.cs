using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lvgaga.Controllers
{
    [RoutePrefix("comments")]
    public class CommentController : Controller
    {
        // GET: comment
        [Route]
        public ActionResult Index()
        {
            return View();
        }

        [Route("{id}")]
        // GET: comments/5
        public ActionResult Show(int id)
        {
            return View();
        }
    }
}
