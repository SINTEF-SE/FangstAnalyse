using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.Commands;
using Fiskinfo.Fangstanalyse.API.Constants;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiskinfo.Fangstanalyse.API.Controllers
{
/*    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    public class HighPerformanceController
    {
        /// <summary>
        /// UH OH SPAGHETTI-O
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the catch report or a 404 Not Found if a catch report with the specified unique
        /// identifier was not found.</returns>
        [HttpGet(Name = "mordi")]
        [SwaggerResponse(StatusCodes.Status200OK, "Cool beans")]
        public Task<IActionResult> Get(
            [FromServices] IGenerateHighPerformanceSqlCommand command,
            CancellationToken cancellationToken) => command.ExecuteAsync(cancellationToken);
    }*/
}