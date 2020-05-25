using System.ComponentModel.DataAnnotations;

namespace Fiskinfo.Fangstanalyse.Infrastructure.Data.Models
{
    public class WindDatum
    {
        [Key] // TODO: REMOVE ID AND CREATE A COMPOSITE KEY of DATETIME, LAT, LON
        // https://docs.microsoft.com/en-us/ef/core/modeling/keys
        public int Id { get; set; }
        public string datetime { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double wind_direction_10m { get; set; }
        public double wind_speed_10m { get; set; }
    }
}