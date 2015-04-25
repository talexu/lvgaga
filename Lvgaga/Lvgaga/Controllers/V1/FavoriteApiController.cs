using System.Threading.Tasks;
using System.Web.Http;
using LvService.Factories.Uri;
using LvService.Services;
using Microsoft.AspNet.Identity;

namespace Lvgaga.Controllers.V1
{
    [RoutePrefix("api/v1/favorites")]
    public class FavoriteApiController : ApiController
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IUriFactory _uriFactory;

        public FavoriteApiController()
        {

        }

        public FavoriteApiController(IFavoriteService favoriteService, IUriFactory uriFactory)
        {
            _favoriteService = favoriteService;
            _uriFactory = uriFactory;
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

        [HttpDelete]
        [Authorize]
        [Route("{partitionKey}/{rowKey}")]
        public async Task<IHttpActionResult> Delete(string partitionKey, string rowKey)
        {
            await _favoriteService.DeleteFavoriteAsync(User.Identity.GetUserId(), partitionKey, rowKey);
            return Ok();
        }
    }
}
