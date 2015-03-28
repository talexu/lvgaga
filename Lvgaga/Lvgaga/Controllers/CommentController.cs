using System.Threading.Tasks;
using System.Web.Mvc;
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

        // GET: Comment
        [Route("{partitionKey}/{rowKey}")]
        public async Task<ActionResult> Get(string partitionKey, string rowKey)
        {
            return View(await _commentService.GetCommentsAsync(partitionKey, rowKey, 20));
        }
    }
}