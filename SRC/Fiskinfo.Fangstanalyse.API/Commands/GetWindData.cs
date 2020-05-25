using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using SintefSecure.Framework.SintefSecure.AspNetCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetWindData : IAsyncCommand<string, string> {}
    public class GetWindData : IGetWindData
    {
        private IConfiguration _configuration;
        private const string _dataFilePrefix = "met_analysis_1_0km_nordic_";
        private const string _dataFileSuffix = "_averaged.csv";
        private static string _windRepositoryLocation = null;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GetWindData(IActionContextAccessor actionContextAccessor, FangstanalyseContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _windRepositoryLocation = _configuration.GetSection("DataPaths").GetValue<string>("MonthlyAverageWindRepositoryRootPath");
        }

        public async Task<IActionResult> ExecuteAsync(string years, string months, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_windRepositoryLocation))
            {
                throw new DirectoryNotFoundException();
            }

            List<ReducedWindDataViewModel> data = new List<ReducedWindDataViewModel>();
            var httpContext = _actionContextAccessor.ActionContext.HttpContext;

            // IF THEY HAVE IT CACHED AND IS JUST CHECKING, WE DONT UPDATE HISTORICAL DATA SO JUST return 304
            if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var stringValues))
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }

            int[] yearsArray = years.Split("-").Select(int.Parse).ToArray();
            int[] monthsArray = months.Split("-").Select(int.Parse).ToArray();

            for(int i = 0; i < yearsArray.Length; i++)
            {
                for(int j = 0; j < monthsArray.Length; j++)
                {
                    string month = monthsArray[j] < 10 ? $"0{Convert.ToString(monthsArray[j])}" : Convert.ToString(monthsArray[j]);
                    DateTime dateTime = DateTime.Parse($"{yearsArray[i]}-{month}-01T00:00:00.000Z");
                    string filename = $"{_dataFilePrefix}{yearsArray[i]}{month}{_dataFileSuffix}";

                    var windDataFilePath = Path.Combine(_windRepositoryLocation, filename);

                    if (!string.IsNullOrEmpty(windDataFilePath) && File.Exists(windDataFilePath))
                    {
                        data.AddRange(fetchCSVFile(windDataFilePath, dateTime));
                    }
                }
            }

            return new OkObjectResult(data);
        }

        public List<ReducedWindDataViewModel> fetchCSVFile(string filePath, DateTime dateTime)
        {
            List<ReducedWindDataViewModel> resultData = new List<ReducedWindDataViewModel>();
            StreamReader reader = new StreamReader(File.OpenRead(filePath));

            if (!reader.EndOfStream)
            {
                reader.ReadLine(); // Skip header
            }

            //string vara1, vara2, vara3, vara4;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (!String.IsNullOrWhiteSpace(line))
                {
                    var lineArray = line.Split(',');

                    resultData.Add(new ReducedWindDataViewModel()
                    {
                        datetime = dateTime,
                        latitude = Convert.ToDouble(lineArray[0], CultureInfo.InvariantCulture),
                        longitude = Convert.ToDouble(lineArray[1], CultureInfo.InvariantCulture),
                        wind_direction_10m = Convert.ToDouble(lineArray[2], CultureInfo.InvariantCulture),
                        wind_speed_10m = Convert.ToDouble(lineArray[3], CultureInfo.InvariantCulture)
                    });
                }
            }

            return resultData;
        }
    }
}