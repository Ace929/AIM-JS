using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EndOfDayReportWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Register your background worker
                    services.AddHostedService<Worker>();
                })
                // If you want to run as a Windows Service:
                // .UseWindowsService()
                ;
    }
}
