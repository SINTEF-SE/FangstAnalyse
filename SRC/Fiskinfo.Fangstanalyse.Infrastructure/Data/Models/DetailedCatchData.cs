using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiskinfo.Fangstanalyse.Infrastructure.Data.Models
{
    [Table("reduced_catch_data")]
    public class DetailedCatchData
    {
        [Key]
        public int pk_reduced_catch_data { get; set; }
        public double rundvekt { get; set; }
        public string fangstfelt { get; set; }
        public string art { get; set; }
        public int art_kode { get; set; }
        public string lengdegruppe { get; set; }
        public int lengdekode { get; set; }
        public string kvalitet { get; set; }
        public int kvalitetkode { get; set; }
        public string redskap { get; set; }
        public int redskap_kode { get; set; }
        public DateTime timestamp_landing { get; set; }
        public int dokument_versjonsnummer { get; set; }
        public string dokument_versjonstidspunkt { get; set; }
        public Int64 dokumentnummer { get; set; }
        public int linjenummer { get; set; }
        public string fartoy_navn { get; set; }
        public string fartoy_kommune { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public string year_and_month { get; set; }
    }
}
