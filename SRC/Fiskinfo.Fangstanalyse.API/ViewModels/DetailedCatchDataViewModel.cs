using System;

namespace Fiskinfo.Fangstanalyse.API.ViewModels
{
    public class DetailedCatchDataViewModel
    {
        public DateTime salgsdato { get; set; }
        public int dokument_versjonsnummer { get; set; }
        public Int64 dokumentnummer { get; set; }
        public int linjenummer { get; set; }
        public string fartoynavn { get; set; }
        public string fartoykommune { get; set; }
        public string lengde { get; set; }
        public string fangstfelt { get; set; }
        public string art { get; set; }
        public string kvalitet { get; set; }
        public string redskap { get; set; }
        public double rundvekt { get; set; }
    }
}
