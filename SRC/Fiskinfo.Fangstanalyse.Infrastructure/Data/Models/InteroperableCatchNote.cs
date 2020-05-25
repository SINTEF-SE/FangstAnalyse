using System.ComponentModel.DataAnnotations;

namespace Fiskinfo.Fangstanalyse.API.ViewModels
{
    //Names and everything here is copied from Bårds old model, not to mess anything up during the transitional stage(s)
    public class InteroperableCatchNote
    {
        [Key]
        public int Id { get; set; }
        public string dokumentsalgsdato { get; set; }
        public double? lengdegruppekode { get; set; }
        public string redskap { get; set; }
        public string fangstfelt { get; set; }
        public string art { get; set; }
        public long? kvalitetkode { get; set; }
        public double? rundvekt { get; set; }
    }
}