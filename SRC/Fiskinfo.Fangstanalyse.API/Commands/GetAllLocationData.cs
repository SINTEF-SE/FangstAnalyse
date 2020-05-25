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
    public interface IGetAllLocationData : IAsyncCommand<int> {}
    
    public class GetAllLocationData
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;
        
        public GetAllLocationData(IActionContextAccessor actionContextAccessor, FangstanalyseContext context)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(string lok, CancellationToken cancellationToken)
        {
            var retval = await _context.FangstDataRaw.Where(a => a.Lok == lok).FirstOrDefaultAsync(cancellationToken);
            if (retval == null)
            {
                return new NotFoundResult();
            }
            
            var httpContext = _actionContextAccessor.ActionContext.HttpContext;
            if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var stringValues))
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }
            return new OkObjectResult(retval);
        }
        
    }
}