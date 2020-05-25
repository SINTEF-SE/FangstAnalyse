using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Fiskinfo.Fangstanalyse.API2.Commands
{
    public class DeleteCarCommand : IDeleteCarCommand
    {
        private readonly ICarRepository carRepository;

        public DeleteCarCommand(ICarRepository carRepository) =>
            this.carRepository = carRepository;

        public async Task<IActionResult> ExecuteAsync(int carId, CancellationToken cancellationToken)
        {
            var car = await this.carRepository.GetAsync(carId, cancellationToken);
            if (car is null)
            {
                return new NotFoundResult();
            }

            await this.carRepository.DeleteAsync(car, cancellationToken);

            return new NoContentResult();
        }
    }
}