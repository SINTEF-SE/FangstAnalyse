using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetAllOptimizedCatchDataCommand : IAsyncCommand{}
    
    public class GetAllOptimizedCatchDataCommand : IGetAllOptimizedCatchDataCommand
    {
        private readonly FangstanalyseContext _context;
        
        public GetAllOptimizedCatchDataCommand(FangstanalyseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var retval = await _context.OptimizedCatchData.Select(x => new OptimizedCatchDataViewModel()
            {
                rundvekt = x.rundvekt,
                fangstfelt = x.fangstfelt,
                art = x.art,
                dato = x.dato,
                lengdegruppe = x.lengdekode,
                kvalitetkode = x.kvalitetkode,
                redskapkode = x.redskap
            }).ToListAsync(cancellationToken);
            return new OkObjectResult(retval);
        }
    }
}