using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsImport.Notification.Consumer.Domain.Imports.Handlers;
using ProductsImport.Notification.Consumer.Domain.Imports.Repositories;
using ProductsImport.Notification.Consumer.Infrastructure.Repositories;

namespace ProductsImport.Notification.Consumer
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
                    services.AddHostedService<Worker>();
                    services.AddTransient<IImportCompletedHandler, ImportCompletedHandler>();
                    services.AddTransient<IImportRepository, ImportRepository>();
                });
    }
}
