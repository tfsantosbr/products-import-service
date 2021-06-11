using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductsImport.Consumer.Domain.Imports.Events;
using ProductsImport.Consumer.Domain.Imports.Handlers;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ProductsImport.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IImportCreatedHandler _importCreatedHandler;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IImportCreatedHandler importCreatedHandler, IConfiguration configuration)
        {
            _logger = logger;
            _importCreatedHandler = importCreatedHandler;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _configuration["Kafka:GroupId"],
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("products-import-created");

            try
            {
                _logger.LogInformation("Products Import started. Waiting for messages...");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var importCreated = JsonSerializer.Deserialize<ImportCreated>(result.Message.Value);

                        await _importCreatedHandler.Handle(importCreated);
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
