using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API2.Repositories;
using Fiskinfo.Fangstanalyse.API2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using SIntefSecureFramework.Mapping;
using Car = Fiskinfo.Fangstanalyse.API2.Models.Car;

namespace Fiskinfo.Fangstanalyse.API2.Commands
{
    public class PutCarCommand : IPutCarCommand
    {
        private readonly ICarRepository carRepository;
        private readonly IMapper<Car, ViewModels.Car> carToCarMapper;
        private readonly IMapper<SaveCar, Car> saveCarToCarMapper;

        public PutCarCommand(
            ICarRepository carRepository,
            IMapper<Car, ViewModels.Car> carToCarMapper,
            IMapper<SaveCar, Car> saveCarToCarMapper)
        {
            this.carRepository = carRepository;
            this.carToCarMapper = carToCarMapper;
            this.saveCarToCarMapper = saveCarToCarMapper;
        }

        public async Task<IActionResult> ExecuteAsync(int carId, SaveCar saveCar, CancellationToken cancellationToken)
        {
            var car = await this.carRepository.GetAsync(carId, cancellationToken);
            if (car is null)
            {
                return new NotFoundResult();
            }

            this.saveCarToCarMapper.Map(saveCar, car);
            car = await this.carRepository.UpdateAsync(car, cancellationToken);
            var carViewModel = this.carToCarMapper.Map(car);

            return new OkObjectResult(carViewModel);
        }
    }
}