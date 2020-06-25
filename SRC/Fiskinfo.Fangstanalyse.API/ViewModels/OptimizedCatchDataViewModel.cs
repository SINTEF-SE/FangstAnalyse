using System;
using Fiskinfo.Fangstanalyse.API.ViewModelSchemaFilters;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiskinfo.Fangstanalyse.API.ViewModels
{
    [SwaggerSchemaFilter(typeof(OptimizedCatchDataViewModelSchemaFilter))]
    public class OptimizedCatchDataViewModel
    {
        public double rundvekt { get; set; }
        public string fangstfelt { get; set; }
        public string art { get; set; }
        public DateTime dato { get; set; }
        public int lengdegruppe { get; set; }
        public int kvalitetkode { get; set; }
        public int redskapkode { get; set; }
        public double temperatur { get; set; }
        public double lufttrykk{ get; set; }

        
        public string[] GetCsvHeaders()
        {
            return new[] { "rundvekt", "fangstfelt", "art", "dato", "lengdegruppe", "kvalitetkode", "redskapkode", "temperatur", "lufttrykk" + "\n"};
        }
        public string GetFormattedCsvLine()
        {
            return $"{rundvekt}, {fangstfelt}, {art}, {dato}, {lengdegruppe}, {kvalitetkode}, {redskapkode}, {temperatur}, {lufttrykk} + \n";
        }
    }
}