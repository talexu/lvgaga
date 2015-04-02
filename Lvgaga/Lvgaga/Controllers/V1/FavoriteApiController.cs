using System.Threading.Tasks;
using System.Web.Http;
using LvService.Services;
using Microsoft.AspNet.Identity;

namespace Lvgaga.Controllers.V1
{
    [RoutePrefix("api/v1/favorites")]
    public class FavoriteApiController : ApiController
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteApiController()
        {

        }

        public FavoriteApiController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        [Authorize]
        [Route("{partitionKey}/{rowKey}")]
        public async Task<IHttpActionResult> Post(string partitionKey, string rowKey)
        {
            var entity = await _favoriteService.CreateFavoriteAsync(User.Identity.GetUserId(), partitionKey, rowKey);
            if (entity != null)
            {
                return Created(Request.RequestUri, entity);
            }
            return BadRequest();
        }
    }
}
