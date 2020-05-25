using System;

namespace Fiskinfo.Fangstanalyse.API.Services
{
    /// <summary>
    /// Retrieves the current time. Helps with unit testing by letting you mock the system clock.
    /// </summary>
    public interface IClockService
    {
        DateTimeOffset UtcNow { get; }
    }
}
