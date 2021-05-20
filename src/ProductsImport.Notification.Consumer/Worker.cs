using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductsImport.Notification.Consumer.Domain.Imports.Events;
using ProductsImport.Notification.Consumer.Domain.Imports.Handlers;

namespace ProductsImport.Notification.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IImportCompletedHandler _importCompletedHandler;

        public Worker(ILogger<Worker> logger, IImportCompletedHandler importCompletedHandler)
        {
            _logger = logger;
            _importCompletedHandler = importCompletedHandler;
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
            consumer.Subscribe("products-import-completed");

            try
            {
                _logger.LogInformation("Products Notification started. Waiting for messages...");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var importCompleted = JsonSerializer.Deserialize<ImportCompleted>(result.Message.Value);

                        await _importCompletedHandler.Handle(importCompleted);
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
