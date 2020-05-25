using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API2.Constants;
using Fiskinfo.Fangstanalyse.API2.Repositories;
using Fiskinfo.Fangstanalyse.API2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using SIntefSecureFramework.Mapping;
using Car = Fiskinfo.Fangstanalyse.API2.Models.Car;

namespace Fiskinfo.Fangstanalyse.API2.Commands
{
    public class PostCarCommand : IPostCarCommand
    {
        private readonly ICarRepository carRepository;
        private readonly IMapper<Car, ViewModels.Car> carToCarMapper;
        private readonly IMapper<SaveCar, Car> saveCarToCarMapper;

        public PostCarCommand(
            ICarRepository carRepository,
            IMapper<Car, ViewModels.Car> carToCarMapper,
            IMapper<SaveCar, Car> saveCarToCarMapper)
        {
            this.carRepository = carRepository;
            this.carToCarMapper = carToCarMapper;
            this.saveCarToCarMapper = saveCarToCarMapper;
        }

        public async Task<IActionResult> ExecuteAsync(SaveCar saveCar, CancellationToken cancellationToken)
        {
            var car = this.saveCarToCarMapper.Map(saveCar);
            car = await this.carRepository.AddAsync(car, cancellationToken);
            var carViewModel = this.carToCarMapper.Map(car);

            return new CreatedAtRouteResult(
                CarsControllerRoute.GetCar,
                new {carId = carViewModel.CarId},
                carViewModel);
        }
    }
}