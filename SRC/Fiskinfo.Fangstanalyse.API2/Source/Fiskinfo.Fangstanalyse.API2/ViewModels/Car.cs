namespace Fiskinfo.Fangstanalyse.API2.ViewModels
{
    /// <summary>
    /// A make and model of car.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// The cars unique identifier.
        /// </summary>
        /// <example>1</example>
        public int CarId { get; set; }

        /// <summary>
        /// The number of cylinders in the cars engine.
        /// </summary>
        /// <example>6</example>
        public int Cylinders { get; set; }

        /// <summary>
        /// The make of the car.
        /// </summary>
        /// <example>Honda</example>
        public string Make { get; set; }

        /// <summary>
        /// The model of the car.
        /// </summary>
        /// <example>Civic</example>
        public string Model { get; set; }

        /// <summary>
        /// The URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        /// <example>/cars/1</example>
        public string Url { get; set; }
    }
}
