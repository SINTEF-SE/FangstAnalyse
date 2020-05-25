using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetOptimizedFieldsCommand : IAsyncCommand<string, string>
    {
    }

    public class GetOptimizedFieldsCommand : IGetOptimizedFieldsCommand
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;

        public GetOptimizedFieldsCommand(IActionContextAccessor actionContextAccessor, FangstanalyseContext context)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
        }

        //MIMIC OF OLD API
        public async Task<IActionResult> ExecuteAsync(string year, string month, CancellationToken cancellationToken)
        {
            var retval = new List<InteroperableCatchNote>();
            foreach (string y in year.Split("-"))
            {
                var stuff = await _context.FangstDataRaw.Where(a =>
                    DateTime.ParseExact(a.DokumentSalgsdato, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).Year == int.Parse(year) &&
                    DateTime.ParseExact(a.DokumentSalgsdato, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).Month == int.Parse(month)
                ).Select(x => new InteroperableCatchNote()
                {
                    lengdegruppekode = x.LengdegruppeKode,
                    redskap = x.Redskap,
                    fangstfelt = x.Lok,
                    art = x.Art,
                    kvalitetkode = x.KvalitetKode,
                    rundvekt = x.Rundvekt
                }).ToListAsync(cancellationToken: cancellationToken);
                retval.AddRange(stuff);
            }

            return new OkObjectResult(retval);
        }

        public async Task<IActionResult> ExecuteAsyncFromTo(string from, string to, CancellationToken cancellationToken)
        {
            var startDate = DateTime.ParseExact(from, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(to, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var result = _context.FangstDataRaw.Where(a =>
                DateTime.ParseExact(a.DokumentSalgsdato, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture) >= startDate &&
                DateTime.ParseExact(a.DokumentSalgsdato, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture) < endDate).Select(x => new InteroperableCatchNote()
            {
                lengdegruppekode = x.LengdegruppeKode,
                redskap = x.Redskap,
                fangstfelt = x.Lok,
                art = x.Art,
                kvalitetkode = x.KvalitetKode,
                rundvekt = x.Rundvekt
            }).ToListAsync(cancellationToken: cancellationToken);
            return new OkObjectResult(result);
        }
    }
}