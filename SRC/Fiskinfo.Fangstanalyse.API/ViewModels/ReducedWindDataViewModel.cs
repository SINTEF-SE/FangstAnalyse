using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiskinfo.Fangstanalyse.API.ViewModels
{
    public class ReducedWindDataViewModel
    {
        public DateTime datetime { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double wind_direction_10m { get; set; }
        public double wind_speed_10m { get; set; }
    }
}
