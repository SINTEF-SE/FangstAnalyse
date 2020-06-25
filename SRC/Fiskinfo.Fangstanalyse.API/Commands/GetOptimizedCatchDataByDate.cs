using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SintefSecure.Framework.SintefSecure.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetOptimizedCatchDataByDate : IAsyncCommand<string, string>
    {
    }

    public class GetOptimizedCatchDataByDate : IGetOptimizedCatchDataByDate
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;
        private readonly IConfiguration _configuration;

        public GetOptimizedCatchDataByDate(IActionContextAccessor actionContextAccessor, FangstanalyseContext context,
            IConfiguration configuration)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
            _configuration = configuration;
        }

        //MIMIC OF OLD API
        public async Task<IActionResult> ExecuteAsync(string year, string month, CancellationToken cancellationToken)
        {
            // var retval = new List<OptimizedCatchData>();
            //          int[] months = month.Split("-").Select(int.Parse).ToArray();
            //         int[] years = year.Split("-").Select(int.Parse).ToArray();
            string compiledYears = year.Replace("-", ",");
            string compiledMonths = month.Replace("-", ",");


            //           Stopwatch sw = new Stopwatch();
            //           sw.Start();
            /*           var retval = await _context.OptimizedCatchData.Where(
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
                      }).AsNoTracking().ToListAsync(cancellationToken);
                      sw.Stop();
          */

            List<OptimizedCatchDataViewModel> retval = new List<OptimizedCatchDataViewModel>();
            using (NpgsqlConnection connection =
                new NpgsqlConnection(_configuration.GetConnectionString("FangstanalyseConnection")))
            {
                string query =
                    $"SELECT CAST(note.rundvekt AS double precision) AS rundvekt, note.fangstfelt, note.art, note.dato, note.lengdekode AS lengdegruppe, note.kvalitetkode, note.redskap AS redskapkode, note.temperatur, note.lufttrykk FROM monthly_aggregated_grouped_catch_data AS note WHERE note.year IN ({compiledYears}) AND note.month IN ({compiledMonths})";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    connection.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retval.Add(new OptimizedCatchDataViewModel()
                            {
                                rundvekt = reader.GetDouble(reader.GetOrdinal("rundvekt")),
                                fangstfelt = reader.GetString(reader.GetOrdinal("fangstfelt")),
                                art = reader.GetString(reader.GetOrdinal("art")),
                                dato = reader.GetDateTime(reader.GetOrdinal("dato")),
                                lengdegruppe = reader.GetInt32(reader.GetOrdinal("lengdegruppe")),
                                kvalitetkode = reader.GetInt32(reader.GetOrdinal("kvalitetkode")),
                                redskapkode = reader.GetInt32(reader.GetOrdinal("redskapkode")),
                                temperatur = reader.GetDouble(reader.GetOrdinal("temperatur")),
                                lufttrykk = reader.GetDouble(reader.GetOrdinal("lufttrykk"))
                            });
                        }
                    }
                }
            }

            //          sw.Stop();
            //         Console.WriteLine("Elapsed time # 1 " + sw.ElapsedMilliseconds);
            return new OkObjectResult(retval);
        }
    }
}