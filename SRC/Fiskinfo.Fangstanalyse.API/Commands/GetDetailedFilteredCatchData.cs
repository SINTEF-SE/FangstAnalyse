using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetDetailedCatchDataByDate : IAsyncCommand<DetailedCatchDataFilterViewModel> {}

    public class GetDetailedFilteredCatchData : IGetDetailedCatchDataByDate
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;

        public GetDetailedFilteredCatchData(IActionContextAccessor actionContextAccessor, FangstanalyseContext context)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync([FromQuery]DetailedCatchDataFilterViewModel filters, CancellationToken cancellationToken)
        {

            if(filters.GetDates().Length == 0 || filters.GetDates().Length > 36)
            {
                return new BadRequestObjectResult(filters.GetDates());
            }

            var retval = await _context.DetailedCatchData.Where(
                note =>
                filters.GetDates().Contains(note.year_and_month) &&
                (filters.GetCatchAreas().Length <= 0 || filters.GetCatchAreas().Contains(note.fangstfelt)) &&
                (filters.GetSpeciesCodes().Length <= 0 || filters.GetSpeciesCodes().Contains(note.art_kode)) &&
                (filters.GetLengthCodes().Length <= 0 || filters.GetLengthCodes().Contains(note.lengdekode)) &&
                (filters.GetQualityCodes().Length <= 0 || filters.GetQualityCodes().Contains(note.kvalitetkode)) &&
                (filters.GetToolCodes().Length <= 0 || filters.GetToolCodes().Contains(note.redskap_kode))
            ).Select(x => new DetailedCatchDataViewModel()
            {
                rundvekt = x.rundvekt,
                fangstfelt = x.fangstfelt,
                art = x.art,
                salgsdato = x.timestamp_landing,
                lengde = x.lengdegruppe,
                kvalitet = x.kvalitet,
                redskap = x.redskap,
                dokumentnummer = x.dokumentnummer,
                dokument_versjonsnummer = x.dokument_versjonsnummer,
                linjenummer = x.linjenummer,
                fartoynavn = x.fartoy_navn,
                fartoykommune = x.fartoy_kommune
            }).ToListAsync(cancellationToken);
            return new OkObjectResult(retval);
        }
    }
}
