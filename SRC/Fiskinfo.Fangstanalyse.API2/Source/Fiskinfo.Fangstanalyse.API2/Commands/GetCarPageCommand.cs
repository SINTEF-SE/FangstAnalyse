using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fiskinfo.Fangstanalyse.API2.Constants;
using Fiskinfo.Fangstanalyse.API2.Repositories;
using Fiskinfo.Fangstanalyse.API2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SintefSecureFramework.AspNetCore;
using SIntefSecureFramework.Mapping;
using Car = Fiskinfo.Fangstanalyse.API2.Models.Car;

namespace Fiskinfo.Fangstanalyse.API2.Commands
{
    public class GetCarPageCommand : IGetCarPageCommand
    {
        private const string LinkHttpHeaderName = "Link";
        private const int DefaultPageSize = 3;
        private readonly ICarRepository carRepository;
        private readonly IMapper<Car, ViewModels.Car> carMapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public GetCarPageCommand(
            ICarRepository carRepository,
            IMapper<Car, ViewModels.Car> carMapper,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.carRepository = carRepository;
            this.carMapper = carMapper;
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        public async Task<IActionResult> ExecuteAsync(PageOptions pageOptions, CancellationToken cancellationToken)
        {
            pageOptions.First = !pageOptions.First.HasValue && !pageOptions.Last.HasValue ? DefaultPageSize : pageOptions.First;
            var createdAfter = Cursor.FromCursor<DateTimeOffset?>(pageOptions.After);
            var createdBefore = Cursor.FromCursor<DateTimeOffset?>(pageOptions.Before);

            var getCarsTask = this.GetCarsAsync(pageOptions.First, pageOptions.Last, createdAfter, createdBefore, cancellationToken);
            var getHasNextPageTask = this.GetHasNextPageAsync(pageOptions.First, createdAfter, createdBefore, cancellationToken);
            var getHasPreviousPageTask = this.GetHasPreviousPageAsync(pageOptions.Last, createdAfter, createdBefore, cancellationToken);
            var totalCountTask = this.carRepository.GetTotalCountAsync(cancellationToken);

            await Task.WhenAll(getCarsTask, getHasNextPageTask, getHasPreviousPageTask, totalCountTask);
            var cars = await getCarsTask;
            var hasNextPage = await getHasNextPageTask;
            var hasPreviousPage = await getHasPreviousPageTask;
            var totalCount = await totalCountTask;

            if (cars is null)
            {
                return new NotFoundResult();
            }

            var (startCursor, endCursor) = Cursor.GetFirstAndLastCursor(cars, x => x.Created);
            var carViewModels = this.carMapper.MapList(cars);

            var connection = new Connection<ViewModels.Car>()
            {
                Items = carViewModels,
                PageInfo = new PageInfo()
                {
                    Count = carViewModels.Count,
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    NextPageUrl = hasNextPage
                        ? new Uri(this.linkGenerator.GetUriByRouteValues(
                            this.httpContextAccessor.HttpContext,
                            CarsControllerRoute.GetCarPage,
                            new PageOptions()
                            {
                                First = pageOptions.First,
                                Last = pageOptions.Last,
                                After = endCursor,
                            }))
                        : null,
                    PreviousPageUrl = hasPreviousPage
                        ? new Uri(this.linkGenerator.GetUriByRouteValues(
                            this.httpContextAccessor.HttpContext,
                            CarsControllerRoute.GetCarPage,
                            new PageOptions()
                            {
                                First = pageOptions.First,
                                Last = pageOptions.Last,
                                Before = startCursor
                            }))
                        : null,
                    FirstPageUrl = new Uri(this.linkGenerator.GetUriByRouteValues(
                        this.httpContextAccessor.HttpContext,
                        CarsControllerRoute.GetCarPage,
                        new PageOptions()
                        {
                            First = pageOptions.First ?? pageOptions.Last,
                        })),
                    LastPageUrl = new Uri(this.linkGenerator.GetUriByRouteValues(
                        this.httpContextAccessor.HttpContext,
                        CarsControllerRoute.GetCarPage,
                        new PageOptions()
                        {
                            Last = pageOptions.First ?? pageOptions.Last,
                        })),
                },
                TotalCount = totalCount,
            };

            this.httpContextAccessor.HttpContext.Response.Headers.Add(
                LinkHttpHeaderName,
                connection.PageInfo.ToLinkHttpHeaderValue());

            return new OkObjectResult(connection);
        }

        private Task<List<Car>> GetCarsAsync(
            int? first,
            int? last,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken)
        {
            Task<List<Car>> getCarsTask;
            if (first.HasValue)
            {
                getCarsTask = this.carRepository.GetCarsAsync(first, createdAfter, createdBefore, cancellationToken);
            }
            else
            {
                getCarsTask = this.carRepository.GetCarsReverseAsync(last, createdAfter, createdBefore, cancellationToken);
            }

            return getCarsTask;
        }

        private async Task<bool> GetHasNextPageAsync(
            int? first,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken)
        {
            if (first.HasValue)
            {
                return await this.carRepository.GetHasNextPageAsync(first, createdAfter, cancellationToken);
            }
            else if (createdBefore.HasValue)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> GetHasPreviousPageAsync(
            int? last,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken)
        {
            if (last.HasValue)
            {
                return await this.carRepository.GetHasPreviousPageAsync(last, createdBefore, cancellationToken);
            }
            else if (createdAfter.HasValue)
            {
                return true;
            }

            return false;
        }
    }
}