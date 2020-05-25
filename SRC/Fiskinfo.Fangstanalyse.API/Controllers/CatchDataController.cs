using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.Commands;
using Fiskinfo.Fangstanalyse.API.Constants;
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
    public class CatchDataController : ControllerBase
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
        /// Returns an Allow HTTP header with the allowed HTTP methods for a car with the specified unique identifier.
        /// </summary>
        /// <param name="catchReportId"> The catch reports unique identifier</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{catchReportId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options(int catchReportId)
        {
            HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Delete,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Patch,
                HttpMethods.Post,
                HttpMethods.Put);
            return Ok();
        }
        
        /// <summary>
        /// Gets the catch report with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="catchReportId">The catch reports' unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the catch report or a 404 Not Found if a catch report with the specified unique
        /// identifier was not found.</returns>
        [HttpGet("{catchReportId}", Name = CatchDataControllerRoute.GetCatchData)]
        [HttpHead("{catchReportId}", Name = CatchDataControllerRoute.HeadCatchData)]
        [SwaggerResponse(StatusCodes.Status200OK, "The catch report with the specified unique identifier.", typeof(FangstDataRaw))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The catch report has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A catch report with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Get(
            [FromServices] IGetFangstDataRaw command,
            int catchReportId,
            CancellationToken cancellationToken) => command.ExecuteAsync(catchReportId, cancellationToken);
        
        /// <summary>
        /// Get all catch reports with all columns.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing all catch reports or a 404 Not Found if there is no catch reports in the system.</returns>
        [HttpGet(Name = CatchDataControllerRoute.GetCatchDataPage)]
        [SwaggerResponse(StatusCodes.Status200OK, "The catch report with the specified unique identifier.", typeof(List<FangstDataRaw>))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The catch report has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A catch report with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetAll(
            [FromServices] IGetAllFangstDataRaw command,
            CancellationToken cancellationToken) => command.ExecuteAsync(cancellationToken);

        /// <summary>
        /// Get catch reports with a subset of data within a given time range
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="month">Month to fetch data from</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <param name="year">The year to fetch data from</param>
        /// <returns>A 200 OK response containing all catch reports or a 404 Not Found if there is no catch reports in the system.</returns>
        [HttpGet(Name = CatchDataControllerRoute.GetCatchDataInteroperable)]
        [SwaggerResponse(StatusCodes.Status200OK, "The catch report with the specified unique identifier.", typeof(List<FangstDataRaw>))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The catch report has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A catch report with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetCatchDataInteroperable(
            [FromServices] IGetOptimizedFieldsCommand command,
            string year, string month,
            CancellationToken cancellationToken) => command.ExecuteAsync(year,month, cancellationToken);
        
    }
}