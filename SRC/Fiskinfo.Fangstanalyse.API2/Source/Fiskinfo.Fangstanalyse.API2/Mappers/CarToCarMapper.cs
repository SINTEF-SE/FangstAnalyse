using System;
using Fiskinfo.Fangstanalyse.API2.Constants;
using Fiskinfo.Fangstanalyse.API2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SIntefSecureFramework.Mapping;

namespace Fiskinfo.Fangstanalyse.API2.Mappers
{
    public class CarToCarMapper : IMapper<Car, ViewModels.Car>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public CarToCarMapper(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        public void Map(Car source, ViewModels.Car destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.CarId = source.CarId;
            destination.Cylinders = source.Cylinders;
            destination.Make = source.Make;
            destination.Model = source.Model;
            destination.Url = this.linkGenerator.GetUriByRouteValues(
                this.httpContextAccessor.HttpContext,
                CarsControllerRoute.GetCar,
                new {source.CarId});
        }
    }
}