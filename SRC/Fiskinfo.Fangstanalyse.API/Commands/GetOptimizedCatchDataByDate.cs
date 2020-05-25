using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.EvaluatableExpressionFilters.Internal;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetOptimizedCatchDataByDate : IAsyncCommand<string, string>{}
    
    public class GetOptimizedCatchDataByDate : IGetOptimizedCatchDataByDate
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;
        
        public GetOptimizedCatchDataByDate(IActionContextAccessor actionContextAccessor, FangstanalyseContext context)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
        }
        
        //MIMIC OF OLD API
        public async Task<IActionResult> ExecuteAsync(string year, string month, CancellationToken cancellationToken)
        {
            // var retval = new List<OptimizedCatchData>();
            int[] months = month.Split("-").Select(int.Parse).ToArray();
            int[] years = year.Split("-").Select(int.Parse).ToArray();
            
            //This is slower than the foreach version but a bit cleaner imho... TODO: Reeview
            var retval = await _context.OptimizedCatchData.Where(
                note => years.Contains(note.year) && months.Contains(note.month)
            ).Select(x => new OptimizedCatchDataViewModel()
            {
                rundvekt = x.rundvekt,
                fangstfelt = x.fangstfelt,
                art = x.art,
                dato = x.dato,
                lengdegruppe = x.lengdekode,
                kvalitetkode = x.kvalitetkode,
                redskapkode = x.redskap,
                temperatur = x.temperatur,
                lufttrykk = x.lufttrykk
            }).ToListAsync(cancellationToken);

            /*foreach (var y in year.Split("-"))
            {
                var serr = await _context.OptimizedCatchData.Where(
                    note => note.timestamp.Year == int.Parse(y) &&
                            months.Contains(note.timestamp.Month)).ToListAsync(cancellationToken);
                retval.AddRange(serr);
            }*/
            return new OkObjectResult(retval);
        }
    }
}