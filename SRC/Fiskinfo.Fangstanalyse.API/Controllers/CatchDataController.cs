using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.Commands;
using Fiskinfo.Fangstanalyse.API.Constants;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Npgsql;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiskinfo.Fangstanalyse.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    public class CatchDataController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Post);
            return Ok();
        }

        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods for a car with the specified unique identifier.
        /// </summary>
        /// <param name="catchReportId"> The catch reports unique identifier</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{catchReportId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options(int catchReportId)
        {
            HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Delete,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Patch,
                HttpMethods.Post,
                HttpMethods.Put);
            return Ok();
        }

        [HttpGet(Name = CatchDataControllerRoute.GetCatchDataInteroperable + "test")]
        public ActionResult<List<OptimizedCatchDataViewModel>> GetCatchDataUsingSQL([FromServices] IConfiguration configuration, string year, string month)
        {
            string compiledYears = year.Replace("-", ",");
            Stopwatch sw = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            sw.Start();

            string compiledMonths = month.Replace("-", ",");
            List<OptimizedCatchDataViewModel> retval = new List<OptimizedCatchDataViewModel>();
            using (NpgsqlConnection connection =
                new NpgsqlConnection(configuration.GetConnectionString("FangstanalyseConnection")))
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
            StringBuilder sb = new StringBuilder();
            sb.Append(retval[0].GetCsvHeaders());
            sw2.Start();
            foreach (OptimizedCatchDataViewModel viewModel in retval)
            {
                sb.Append(viewModel.GetFormattedCsvLine());
            }

            sw2.Stop();
            sw.Stop();
            Console.WriteLine($"Entire method for datadump took: {sw.ElapsedMilliseconds} Milliseconds");
            Console.WriteLine($"Dumping data as csv and appending to stringbuilder took: {sw2.ElapsedMilliseconds} milliseconds");
            return Content(sb.ToString(), "text/plain");
        }
    }
}