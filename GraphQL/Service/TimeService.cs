namespace GraphQL.Service
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class TimeService : BackgroundService
    {
        private readonly ILogger<TimeService> _logger;

        public TimeService(ILogger<TimeService> logger)
        {
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                this._logger.LogInformation($"[TimeService]: {DateTimeOffset.Now}");
                await Task.Delay(25600, token).ConfigureAwait(false);
            }
        }
    }
}