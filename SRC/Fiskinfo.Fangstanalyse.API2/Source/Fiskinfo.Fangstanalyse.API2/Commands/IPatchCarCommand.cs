using Fiskinfo.Fangstanalyse.API2.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using SintefSecureFramework.AspNetCore;

namespace Fiskinfo.Fangstanalyse.API2.Commands
{
    public interface IPatchCarCommand : IAsyncCommand<int, JsonPatchDocument<SaveCar>>
    {
    }
}