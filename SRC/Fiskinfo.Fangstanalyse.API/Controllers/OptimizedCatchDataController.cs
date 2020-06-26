using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.Commands;
using Fiskinfo.Fangstanalyse.API.Constants;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiskinfo.Fangstanalyse.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    public class OptimizedCatchDataController : ControllerBase
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
        /// Get catch reports with a subset of data within a given time range
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="month">Month to fetch data from</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <param name="year">The year to fetch data from</param>
        /// <returns>A 200 OK response containing all catch reports or a 404 Not Found if there is no catch reports in the system.</returns>
        [HttpGet(Name = OptimizedCatchDataControllerRoute.GetOptimizedCatchData)]
        [SwaggerResponse(StatusCodes.Status200OK, "The catch report with the specified unique identifier.", typeof(List<OptimizedCatchDataViewModel>))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The catch report has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A catch report with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetCatchDataInteroperable(
            [FromServices] IGetOptimizedCatchDataByDate command,
            string year, string month,
            CancellationToken cancellationToken) => command.ExecuteAsync(year,month, cancellationToken);

        /// <summary>
        /// Get detailed catch data matching the given parameters
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="years">The years to fetch data from</param>
        /// <param name="months">The months to fetch data from</param>
        /// <param name="catchAreas">The catch areas to fetch data from</param>
        /// <param name="speciesCodes">The species to get data for</param>
        /// <param name="lengthCodes">The vessel lengths to fetch data for</param>
        /// <param name="qualityCodes">The catch qualities to fetch data for</param>
        /// <param name="toolCodes">The catch tools to fetch data for</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing all catch reports or a 404 Not Found if there is no catch reports in the system.</returns>
        [HttpGet(Name = OptimizedCatchDataControllerRoute.GetDetailedCatchData)]
        [SwaggerResponse(StatusCodes.Status200OK, "Detailed catch records matching the given parameters.", typeof(List<DetailedCatchData>))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The catch data has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No data with the given parameteres could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetDetailedCatchDataInteroperable(
            [FromServices] IGetDetailedCatchDataByDate command,
            [FromQuery]DetailedCatchDataFilterViewModel filters,
            CancellationToken cancellationToken) => command.ExecuteAsync(filters, cancellationToken);
    }
}