using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.Commands;
using Fiskinfo.Fangstanalyse.API.Constants;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiskinfo.Fangstanalyse.API.Controllers
{
    [EnableCors(CorsPolicyName.AllowAny)]
    [Route("[controller]/[action]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    public class ClimateAndWeatherDataController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Post);
            return Ok();
        }

        /// <summary>
        /// Get wind data for a given time
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="years">The year(s) to fetch data from</param>
        /// <param name="months">Month(s) to fetch data from</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing requested wind data or a 404 Not Found if there is no wind data in the system.</returns>
        [HttpGet(Name = ClimateAndWeatherDataControllerRoute.GetWindData)]
        [SwaggerResponse(StatusCodes.Status200OK, "Wind data within the given parameters.", typeof(List<WindDatum>))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The wind data has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No wind data within the given paramaters could be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetWindData(
            [FromServices] IGetWindData command,
            string years, string months,
            CancellationToken cancellationToken) => command.ExecuteAsync(years, months, cancellationToken);
    }
}