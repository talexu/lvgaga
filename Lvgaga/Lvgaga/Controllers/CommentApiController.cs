using System.Threading.Tasks;
using System.Web.Http;
using Lvgaga.App_Start;
using LvModel.View.Comment;
using LvService.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace Lvgaga.Controllers
{
    [RoutePrefix("api/v1/comments")]
    public class CommentApiController : ApiController
    {
        private readonly ICommentService _commentService;

        public CommentApiController()
        {
            _commentService = UnityConfig.GetConfiguredContainer().Resolve<ICommentService>();
        }

        public CommentApiController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="partitionKey">Tumblr的PK, 为媒体类型</param>
        /// <param name="rowKey">Tumblr的RK, 不包括分类前缀, 只包括时间戳</param>
        /// <param name="comment">评论内容</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("{partitionKey}/{rowKey}")]
        public async Task<IHttpActionResult> Post(string partitionKey, string rowKey, [FromBody] PostedComment comment)
        {
            comment.UserId = User.Identity.GetUserId();
            comment.UserName = User.Identity.GetUserName();
            var entity = await _commentService.CreateCommentAsync(rowKey, comment);
            if (entity != null)
            {
                return Created(Request.RequestUri, entity);
            }
            return BadRequest();
        }
    }
}
