using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetAllFangstDataRaw : IAsyncCommand
    {
    }

    public class GetAllFangstDataRaw : IGetAllFangstDataRaw
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;
        
        public GetAllFangstDataRaw(IActionContextAccessor actionContextAccessor, FangstanalyseContext context)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var retval = await _context.FangstDataRaw.ToListAsync(cancellationToken: cancellationToken);
            return new OkObjectResult(retval);
        }
        
    }
}