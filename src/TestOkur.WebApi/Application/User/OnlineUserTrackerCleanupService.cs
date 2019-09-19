namespace TestOkur.WebApi.Application.User
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class OnlineUserTrackerCleanupService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public OnlineUserTrackerCleanupService(ILogger<OnlineUserTrackerCleanupService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Online User Track Cleanup Service is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Online User Track Cleanup Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object state) => OnlineUserTracker.CleanupOldTracks();
    }
}
