using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace StocksServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
                    .WriteTo.Console(LogEventLevel.Information)
                    .CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}