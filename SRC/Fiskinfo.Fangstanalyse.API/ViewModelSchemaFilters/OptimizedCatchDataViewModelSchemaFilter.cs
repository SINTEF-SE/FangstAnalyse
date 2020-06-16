using System;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fiskinfo.Fangstanalyse.API.ViewModelSchemaFilters
{
    public class OptimizedCatchDataViewModelSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            var catchData = new OptimizedCatchDataViewModel()
            {
                rundvekt = 201.23,
                fangstfelt = "2338",
                art = "101",
                dato = DateTime.Now,
                lengdegruppe = 5,
                kvalitetkode = 20,
                redskapkode = 2
            };
            model.Default = catchData;
            model.Example = catchData;
        }
    }
}