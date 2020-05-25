using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiskinfo.Fangstanalyse.Infrastructure.Data.Models
{
    [Table("monthly_aggregated_grouped_catch_data")]
    public class OptimizedCatchData
    {
        [Key]
        public int index { get; set; }
        public int rundvekt { get; set; }
        public string fangstfelt { get; set; }
        public string art { get; set; }
        public DateTime dato { get; set; }
        public int lengdekode { get; set; }
        public int kvalitetkode { get; set; }
        public int redskap { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public double temperatur { get; set; }
        public double lufttrykk { get; set; }
    }
}