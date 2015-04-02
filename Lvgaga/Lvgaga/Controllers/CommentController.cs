using System.Threading.Tasks;
using System.Web.Mvc;
using LvService.Services;
using Microsoft.AspNet.Identity;

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
            var comments = await _commentService.GetCommentModelsAsync(partitionKey, rowKey, 20, User.Identity.GetUserId());
            if (comments != null)
            {
                return View(comments);
            }
            return HttpNotFound();
        }
    }
}