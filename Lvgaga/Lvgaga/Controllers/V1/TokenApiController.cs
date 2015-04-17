using System;
using System.Threading.Tasks;
using System.Web.Http;
using LvModel.Common;
using LvService.Services;
using Microsoft.AspNet.Identity;

namespace Lvgaga.Controllers.V1
{
    [RoutePrefix("api/v1/tokens")]
    public class TokenApiController : ApiController
    {
        private readonly ISasService _sasService;

        public TokenApiController(ISasService sasService)
        {
            _sasService = sasService;
        }

        [HttpGet]
        [Route(LvConstants.TableNameOfTumblr)]
        public async Task<IHttpActionResult> GetSasOfTumblrs()
        {
            return await GetSasOfTable(LvConstants.TableNameOfTumblr);
        }

        [HttpGet]
        [Route(LvConstants.TableNameOfComment)]
        public async Task<IHttpActionResult> GetSasOfComments()
        {
            return await GetSasOfTable(LvConstants.TableNameOfComment);
        }

        [HttpGet]
        [Authorize]
        [Route(LvConstants.TableNameOfFavorite)]
        public async Task<IHttpActionResult> GetSasOfFavorites()
        {
            return await GetSasOfTableWithPartitionKey(LvConstants.TableNameOfFavorite, User.Identity.GetUserId());
        }

        private async Task<IHttpActionResult> GetSasOfTable(string tableName)
        {
            var sas = await _sasService.GetSasForTable(tableName);

            if (!String.IsNullOrEmpty(sas))
            {
                return Ok(sas);
            }
            return NotFound();
        }

        private async Task<IHttpActionResult> GetSasOfTableWithPartitionKey(string tableName, string partitionKey)
        {
            var sas = await _sasService.GetSasForTable(tableName, partitionKey);

            if (!String.IsNullOrEmpty(sas))
            {
                return Ok(sas);
            }
            return NotFound();
        }
    }
}
