namespace Terepnaplo.Contracts
{
    public interface ILocationService
    {
        /// <summary>
        /// Gets the current device location once, prompting for permissions if needed.
        /// </summary>
        /// <param name="highAccuracy">True to use high accuracy (GPS), false to prefer faster/low power.</param>
        /// <param name="timeout">Operation timeout; null uses a sensible default.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The current Location or null if unavailable.</returns>
        Task<Location?> GetCurrentLocationAsync(
            bool highAccuracy = true,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default);
    }
}
