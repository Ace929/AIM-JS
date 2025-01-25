using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EndOfDayReportWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Example: poll the DB to see if a new day’s data is fully loaded
                    if (IsNewDataAvailable())
                    {
                        // Generate + send your EOD report
                        _logger.LogInformation("New data available; sending EOD report.");
                        SendEndOfDayReport();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking for new data or sending report.");
                }

                // Wait 1 minute (or 5, or 15...) before checking again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private bool IsNewDataAvailable()
        {
            // Placeholder:
            // Query the DB to see if the day's data has been inserted
            // Return true if "Yes, it's ready"
            return true; // for testing
        }

        private void SendEndOfDayReport()
        {
            // This is where you put your existing code:
            //  1) Connect to DB
            //  2) Fetch the needed metrics
            //  3) Build the HTML
            //  4) Send the email
        }
    }
}
