using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Fiskinfo.Fangstanalyse.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGenerateHighPerformanceSqlCommand : IAsyncCommand
    {
    }

    public class GenerateHighPerformanceSqlCommand : IGenerateHighPerformanceSqlCommand
    {
        private readonly FangstanalyseContext _context;

        public GenerateHighPerformanceSqlCommand(FangstanalyseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var allData = await  _context.FangstDataRaw.ToListAsync(cancellationToken); // Pardon, lets pray this works, otherwise chunk
            List<InteroperableCatchNote> optimizedData = new List<InteroperableCatchNote>();
            Debug.WriteLine("Actually managed to fetch all data to memory :O");
            foreach (var datum in allData)
            {
                // Check if lok is contained in interoperable catch note
                bool found = false;
                foreach (var entry in optimizedData)
                {
                    if (entry.fangstfelt.Equals(datum.Lok) && entry.art.Equals(datum.Art) && entry.redskap.Equals(datum.Redskap) && entry.kvalitetkode.Equals(datum.KvalitetKode) && entry.dokumentsalgsdato.IsYearMonth(datum.DokumentSalgsdato))
                    {
                        entry.rundvekt += datum.Rundvekt;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                     optimizedData.Add(new InteroperableCatchNote()
                     {
                         lengdegruppekode = datum.LengdegruppeKode,
                         redskap = datum.Redskap,
                         fangstfelt = datum.Lok,
                         art = datum.Art,
                         kvalitetkode = datum.KvalitetKode,
                         rundvekt = datum.Rundvekt
                     });   
                }
            }

            await _context.InteroperableCatchNotes.AddRangeAsync(optimizedData, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(JsonConvert.SerializeObject(new {cool = "ice"}));
        }
    }
}