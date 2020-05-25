using System.Collections.Generic;
using Fiskinfo.Fangstanalyse.API.ViewModelSchemaFilters;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiskinfo.Fangstanalyse.API.ViewModels
{
[SwaggerSchemaFilter(typeof(PageResultCarSchemaFilter))]
public class PageResult<T>
    where T : class
{
    public int Page { get; set; }

    public int Count { get; set; }

    public bool HasNextPage => this.Page < this.TotalPages;

    public bool HasPreviousPage => this.Page > 1;

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public List<T> Items { get; set; }
}
}