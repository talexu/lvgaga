using System.Web.Mvc;

namespace Lvgaga.Controllers
{
    [RoutePrefix("comments")]
    public class CommentController : Controller
    {
        [Route("{id}")]
        // GET: comments/5
        public ActionResult Show(string id)
        {
            return View();
        }
    }
}
