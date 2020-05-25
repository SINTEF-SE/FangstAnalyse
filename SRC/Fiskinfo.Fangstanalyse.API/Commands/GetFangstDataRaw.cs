using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetFangstDataRaw : IAsyncCommand<int>
    {
    }

    public class GetFangstDataRaw : IGetFangstDataRaw
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;

        public GetFangstDataRaw(IActionContextAccessor actionContextAccessor, FangstanalyseContext context)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
        }
        
        public async Task<IActionResult> ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var retval = await _context.FangstDataRaw.FirstOrDefaultAsync(f => f.Index == id, cancellationToken: cancellationToken);
            if (retval == null)
            {
                return new NotFoundResult();
            }
            var httpContext = _actionContextAccessor.ActionContext.HttpContext;
            //TODO ADD modified field and check if modified since for caching, since we're not updating for now can can simply return not modified for modified http requests
            if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var stringValues))
            {
                if (DateTimeOffset.TryParse(stringValues, out var modifiedSince))
                {
                    return new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }
            httpContext.Response.Headers.Add(HeaderNames.LastModified, DateTime.UtcNow.ToString("R"));
            return new OkObjectResult(retval);
        }
    }
}