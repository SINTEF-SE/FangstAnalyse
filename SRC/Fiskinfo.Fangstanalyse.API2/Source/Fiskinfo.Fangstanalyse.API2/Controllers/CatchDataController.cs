using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API2.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiskinfo.Fangstanalyse.API2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
    public class CatchDataController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions(Name = CarsControllerRoute.OptionsCars)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Post);
            return this.Ok();
        }
        
        /// <summary>
        /// Get all catch reports with optimized columns.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing all catch reports or a 404 Not Found if there is no catch reports in the system.</returns>
        [HttpGet(Name = OptimizedCatchDataControllerRoute.GetAllOptimizedCatchData)]
        [SwaggerResponse(StatusCodes.Status200OK, "The catch report with the specified unique identifier.", typeof(List<OptimizedCatchDataViewModel>))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The catch report has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A catch report with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetAll(
            [FromServices] IGetAllOptimizedCatchDataCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync(cancellationToken);
        
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
    }
    }
}