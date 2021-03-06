using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using ProductsImport.Consumer.Domain.Imports.Models;
using ProductsImport.Consumer.Domain.Imports.Repositories;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductsImport.Consumer.Domain.Imports.Services
{
    public class SpreadsheetService : ISpreadsheetService
    {
        private readonly IImportProductRepository _importProductRepository;
        private readonly IProducer<Null, string> _producer;
        private readonly IConfiguration _configuration;

        public SpreadsheetService(IImportProductRepository importProductRepository, IConfiguration configuration)
        {
            _importProductRepository = importProductRepository;
            _configuration = configuration;

            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                EnableDeliveryReports = false
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task<ProcessSpreadsheetResult> Process(Guid importId, string spreadsheetPath)
        {
            int lineNumber = 2;
            int processed = 0;
            int errors = 0;
            var watch = Stopwatch.StartNew();

            using var reader = new StreamReader(spreadsheetPath);

            // pula cabeçalho

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                // Converte linha pra objeto

                var importProduct = ParseLineToImportProduct(importId, line, lineNumber);

                if (importProduct is null)
                {
                    lineNumber++;
                    continue;
                }

                // Grava na tabela de importação

                await _importProductRepository.Create(importProduct);

                // Cria mensagem para processamento do tópico

                await CreateMessage(importProduct);

                lineNumber++;
            }

            // Envia mensagens para o tópico

            RaiseEvents();

            watch.Stop();
            var elapsedMs = watch.Elapsed;

            var result = new ProcessSpreadsheetResult
            {
                TotalLines = lineNumber,
                Processed = processed,
                Errors = errors,
                ProcessTime = elapsedMs.ToString()
            };

            return result;
        }

        private static ImportProduct ParseLineToImportProduct(Guid importId, string line, int lineNumber)
        {
            var values = line.Split(';');

            if (!ValuesValidated(values))
            {
                return null;
            }

            var importProduct = new ImportProduct(
                importId: importId,
                storeId: Convert.ToInt64(values[0]),
                productCode: values[1],
                name: values[3],
                price: Convert.ToDecimal(values[9], CultureInfo.InvariantCulture),
                stock: Convert.ToDecimal(values[14], CultureInfo.InvariantCulture),
                line: lineNumber
                );

            return importProduct;
        }

        private static bool ValuesValidated(string[] values)
        {
            if (values == null || values.Length <= 15)
            {
                return false;
            }

            if (values[0] != null && string.IsNullOrWhiteSpace(values[0]) || !long.TryParse(values[0], out _))
            {
                return false;
            }

            if (values[1] != null && string.IsNullOrWhiteSpace(values[1]) || values[1].Length > 20)
            {
                return false;
            }

            if (values[3] != null && string.IsNullOrWhiteSpace(values[3]) || values[3].Length > 500)
            {
                return false;
            }

            if (values[9] != null && string.IsNullOrWhiteSpace(values[9]) || !decimal.TryParse(values[9], out _))
            {
                return false;
            }

            if (values[14] != null && string.IsNullOrWhiteSpace(values[14]) || !decimal.TryParse(values[14], out _))
            {
                return false;
            }

            return true;
        }

        private async Task CreateMessage(ImportProduct importProduct)
        {
            var json = JsonSerializer.Serialize(importProduct);

            Console.WriteLine(json);

            var message = new Message<Null, string>
            {
                Value = json
            };

            await _producer.ProduceAsync("process-product-requested", message);
        }

        private void RaiseEvents()
        {
            _producer.Flush();
        }
    }
}
