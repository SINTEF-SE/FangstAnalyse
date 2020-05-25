namespace Fiskinfo.Fangstanalyse.API2.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A make and model of car.
    /// </summary>
    public class SaveCar
    {
        /// <summary>
        /// The number of cylinders in the cars engine.
        /// </summary>
        /// <example>6</example>
        [Range(1, 20)]
        public int Cylinders { get; set; }

        /// <summary>
        /// The make of the car.
        /// </summary>
        /// <example>Honda</example>
        [Required]
        public string Make { get; set; }

        /// <summary>
        /// The model of the car.
        /// </summary>
        /// <example>Civic</example>
        [Required]
        public string Model { get; set; }
    }
}
