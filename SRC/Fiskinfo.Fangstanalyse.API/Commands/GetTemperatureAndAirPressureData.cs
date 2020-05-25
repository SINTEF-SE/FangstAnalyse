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

namespace Fiskinfo.Fangstanalyse.API.Commands
{
    public interface IGetTemperatureAndAirPressureData : IAsyncCommand<string, string> { }

    public class GetTemperatureAndAirPressureData : IGetTemperatureAndAirPressureData
    {
        private IConfiguration _configuration;
        private const string _dataFilePrefix = "met_analysis_1_0km_nordic_";
        private const string _dataFileSuffix = "_temp_and_pressure_averaged.csv";
        private static string _dataRepositoryLocation = null;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly FangstanalyseContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GetTemperatureAndAirPressureData(IActionContextAccessor actionContextAccessor, FangstanalyseContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _actionContextAccessor = actionContextAccessor;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _dataRepositoryLocation = _configuration.GetSection("DataPaths").GetValue<string>("MonthlyAverageTemperatureAndAirPressureRootPath");
        }

        public async Task<IActionResult> ExecuteAsync(string years, string months, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_dataRepositoryLocation))
            {
                throw new DirectoryNotFoundException();
            }

            List<TemperatureAndAirPressureViewModel> data = new List<TemperatureAndAirPressureViewModel>();
            var httpContext = _actionContextAccessor.ActionContext.HttpContext;

            // IF THEY HAVE IT CACHED AND IS JUST CHECKING, WE DONT UPDATE HISTORICAL DATA SO JUST return 304
            if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var stringValues))
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }

            int[] yearsArray = years.Split("-").Select(int.Parse).ToArray();
            int[] monthsArray = months.Split("-").Select(int.Parse).ToArray();

            for (int i = 0; i < yearsArray.Length; i++)
            {
                for (int j = 0; j < monthsArray.Length; j++)
                {
                    string month = monthsArray[j] < 10 ? $"0{Convert.ToString(monthsArray[j])}" : Convert.ToString(monthsArray[j]);
                    string dateTimeString = $"{yearsArray[i]}-{month}-01T00:00:00.000Z";
                    string filename = $"{_dataFilePrefix}{yearsArray[i]}{month}{_dataFileSuffix}";

                    var dataFilePath = Path.Combine(_dataRepositoryLocation, filename);

                    if (!string.IsNullOrEmpty(dataFilePath) && File.Exists(dataFilePath))
                    {
                        data.AddRange(fetchCSVFile(dataFilePath, dateTimeString));
                    }
                }
            }

            return new OkObjectResult(data);
        }

        public List<TemperatureAndAirPressureViewModel> fetchCSVFile(string filePath, string dateTime)
        {
            List<TemperatureAndAirPressureViewModel> resultData = new List<TemperatureAndAirPressureViewModel>();
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

                    resultData.Add(new TemperatureAndAirPressureViewModel()
                    {
                        datetime = dateTime,
                        latitude = Convert.ToDouble(lineArray[0], CultureInfo.InvariantCulture),
                        longitude = Convert.ToDouble(lineArray[1], CultureInfo.InvariantCulture),
                        temperature = Convert.ToDouble(lineArray[2], CultureInfo.InvariantCulture),
                        air_pressure = Convert.ToDouble(lineArray[3], CultureInfo.InvariantCulture)
                    });
                }
            }

            return resultData;
        }
    }
}
