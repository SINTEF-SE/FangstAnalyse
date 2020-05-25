﻿﻿using System;
 using System.Linq;

namespace Fiskinfo.Fangstanalyse.API.ViewModels
{
    public class DetailedCatchDataFilterViewModel
    {
        
        public string dates { get; set; }
        public string catchAreas { get; set; }
        public string speciesCodes { get; set; }
        public string lengthCodes { get; set; }
        public string qualityCodes { get; set; }
        public string toolCodes { get; set; }
        
        public DetailedCatchDataFilterViewModel()
        {
            dates = "";
            catchAreas = "";
            speciesCodes = "";
            lengthCodes = "";
            qualityCodes = "";
            toolCodes = "";
        }

        public string[] GetDates()
        {
            return dates.Split(',').Where(str => str.Length > 0).ToArray();
        }

        public string[] GetCatchAreas()
        {
            return catchAreas.Split(',').Where(str => str.Length > 0).ToArray();
        }

        public int[] GetSpeciesCodes()
        {
            return speciesCodes.Split(',').Where(numString => numString.Length > 0).Select(int.Parse).ToArray();
        }

        public int[] GetLengthCodes()
        {
            return lengthCodes.Split(',').Where(numString => numString.Length > 0).Select(int.Parse).ToArray();
        }
        
        public int[] GetQualityCodes()
        {
            return qualityCodes.Split(',').Where(numString => numString.Length > 0).Select(int.Parse).ToArray();
        }
        
        public int[] GetToolCodes()
        {
            return toolCodes.Split(',').Where(numString => numString.Length > 0).Select(int.Parse).ToArray();
        }
        
    }
}
