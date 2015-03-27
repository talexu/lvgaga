using System.Threading.Tasks;
using System.Web.Mvc;
using LvModel.Common;
using LvService.Services;

namespace Lvgaga.Controllers
{
    [RoutePrefix("comments")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Route("{id}")]
        // GET: comments/5
        public async Task<ActionResult> Show(string id)
        {
            return View(await _commentService.GetCommentsAsync(LvConstants.PartitionKeyOfImage, id, 20));
        }
    }
}
