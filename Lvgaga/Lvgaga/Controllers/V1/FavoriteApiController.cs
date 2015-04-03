using System.Linq;
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

        [HttpGet]
        [Authorize]
        [Route("{partitionKey}")]
        public async Task<IHttpActionResult> Get(string partitionKey, string from, string to)
        {
            var entities =
                await _favoriteService.GetFavoriteTumblrModelsAsync(User.Identity.GetUserId(), partitionKey, from, to);
            if (entities != null && entities.Any())
            {
                return Ok(entities.ToDictionary(e => _uriFactory.GetInvertedTicksFromTumblrRowKey(e.RowKey),
                    e => e.MediaType));
            }
            return NotFound();
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
