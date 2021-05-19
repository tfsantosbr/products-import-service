using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Events;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Handlers;

namespace ProductsImport.ProductUpdater.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProcessProductRequestedHandler _processProductRequestedHandler;

        public Worker(ILogger<Worker> logger, IProcessProductRequestedHandler processProductRequestedHandler)
        {
            _logger = logger;
            _processProductRequestedHandler = processProductRequestedHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "products-import-consumer",
                BootstrapServers = "localhost:9091,localhost:9092,localhost:9093",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("process-product-requested");

            try
            {
                _logger.LogInformation("Products Updater started. Waiting for messages...");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var processProductRequested = JsonSerializer.Deserialize<ProcessProductRequested>(result.Message.Value);

                        _logger.LogInformation($"Product Requested: {processProductRequested.StoreId} | {processProductRequested.ProductCode}");

                        await _processProductRequestedHandler.Handle(processProductRequested);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Products Import: Consumer terminated");

                consumer.Close();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Got exception on start up." +
                    $"Exception is {exception.Message}. Stopping service.");
            }
        }
    }
}
