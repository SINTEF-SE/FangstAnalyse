using System;
using Fiskinfo.Fangstanalyse.API2.Services;
using Fiskinfo.Fangstanalyse.API2.ViewModels;
using SIntefSecureFramework.Mapping;
using Car = Fiskinfo.Fangstanalyse.API2.Models.Car;

namespace Fiskinfo.Fangstanalyse.API2.Mappers
{
    public class CarToSaveCarMapper : IMapper<Car, SaveCar>, IMapper<SaveCar, Car>
    {
        private readonly IClockService clockService;

        public CarToSaveCarMapper(IClockService clockService) =>
            this.clockService = clockService;

        public void Map(Car source, SaveCar destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Cylinders = source.Cylinders;
            destination.Make = source.Make;
            destination.Model = source.Model;
        }

        public void Map(SaveCar source, Car destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var now = this.clockService.UtcNow;

            if (destination.Created == DateTimeOffset.MinValue)
            {
                destination.Created = now;
            }

            destination.Cylinders = source.Cylinders;
            destination.Make = source.Make;
            destination.Model = source.Model;
            destination.Modified = now;
        }
    }
}