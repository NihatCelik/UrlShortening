using Business.Handlers.Urls.Commands;
using Business.Handlers.Urls.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : BaseApiController
    {
        [HttpGet("getlongurlbyshorturl")]
        public async Task<IActionResult> GetLongUrlByShortUrl(string shortUrl)
        {
            return GetResponse(await Mediator.Send(new GetUrlQuery { ShortUrl = shortUrl }));
        }

        [HttpPost("createshorturl")]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlCommand createShortUrl)
        {
            return GetResponse(await Mediator.Send(createShortUrl));
        }

        [HttpPost("createcustomurl")]
        public async Task<IActionResult> CreateCustomUrl([FromBody] CreateCustomUrlCommand createCustomUrl)
        {
            return GetResponse(await Mediator.Send(createCustomUrl));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteUrlCommand deleteUrl)
        {
            return GetResponse(await Mediator.Send(deleteUrl));
        }
    }
}
