using System.Threading.Tasks;
using System.Web.Mvc;
using LvService.Common;
using LvService.Services;

namespace Lvgaga.Controllers
{
    [RoutePrefix("comments")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController()
        {

        }

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: Comment
        [Route("{partitionKey}/{rowKey}")]
        public async Task<ActionResult> Get(string partitionKey, string rowKey)
        {
            var comments = await _commentService.GetCommentModelsAsync(partitionKey, rowKey, LvConfiguration.TakingCount);
            if (comments != null)
            {
                return View(comments);
            }
            return HttpNotFound();
        }
    }
}