using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsImport.Consumer.Domain.Imports.Handlers;
using ProductsImport.Consumer.Domain.Imports.Repositories;
using ProductsImport.Consumer.Domain.Imports.Services;
using ProductsImport.Consumer.Infrastructure.Repositories;

namespace ProductsImport.Consumer
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

                    services.AddTransient<IImportCreatedHandler, ImportCreatedHandler>();
                    services.AddTransient<ISpreadsheetService, SpreadsheetService>();
                    services.AddTransient<IImportProductRepository, ImportProductRepository>();
                });
    }
}
