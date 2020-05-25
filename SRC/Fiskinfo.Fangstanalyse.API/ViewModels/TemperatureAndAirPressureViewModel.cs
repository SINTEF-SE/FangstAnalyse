using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiskinfo.Fangstanalyse.API.ViewModels
{
    public class TemperatureAndAirPressureViewModel
    {
        public string datetime { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double temperature { get; set; }
        public double air_pressure { get; set; }
    }
}
