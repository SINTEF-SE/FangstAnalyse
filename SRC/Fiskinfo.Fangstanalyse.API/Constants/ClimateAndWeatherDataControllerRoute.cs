using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiskinfo.Fangstanalyse.API.Constants
{
    public class ClimateAndWeatherDataControllerRoute
    {
        public const string GetWindData = ControllerName.ClimateAndWeatherData + nameof(GetWindData);
        public const string GetTemperatureAndAirPressureData = ControllerName.ClimateAndWeatherData + nameof(GetTemperatureAndAirPressureData);
    }
}
