using Fiskinfo.Fangstanalyse.API.Commands;
using Fiskinfo.Fangstanalyse.API.Services;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SintefSecure.Framework.SintefSecure.Mapping;

namespace Fiskinfo.Fangstanalyse.API
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods add project services.
    /// </summary>
    /// <remarks>
    /// AddSingleton - Only one instance is ever created and returned.
    /// AddScoped - A new instance is created and returned for each request/response cycle.
    /// AddTransient - A new instance is created and returned each time.
    /// </remarks>
    public static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services
                .AddScoped<IGetOptimizedCatchDataByDate, GetOptimizedCatchDataByDate>()
                .AddScoped<IGetWindData, GetWindData>()
                .AddScoped<IGetDetailedCatchDataByDate, GetDetailedFilteredCatchData>();

        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>();
    }
}