using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsImport.ProductUpdater.Consumer.Domain.Imports.Repositories;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Handlers;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Repositories;
using ProductsImport.ProductUpdater.Consumer.Infrastructure.Repositories;

namespace ProductsImport.ProductUpdater.Consumer
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
                    services.AddTransient<IProcessProductRequestedHandler, ProcessProductRequestedHandler>();
                    services.AddTransient<IImportRepository, ImportRepository>();
                    services.AddTransient<IProductRepository, ProductRepository>();
                });
    }
}
