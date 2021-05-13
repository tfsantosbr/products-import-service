using Confluent.Kafka;
using ProductsImport.Consumer.Domain.Imports.Models;
using ProductsImport.Consumer.Domain.Imports.Repositories;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductsImport.Consumer.Domain.Imports.Services
{
    public class SpreadsheetService : ISpreadsheetService
    {
        private readonly IImportProductRepository _importProductRepository;
        private readonly IProducer<Null, string> _producer;

        public SpreadsheetService(IImportProductRepository importProductRepository)
        {
            _importProductRepository = importProductRepository;

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9091,localhost:9092,localhost:9093",
                EnableDeliveryReports = false
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task<ProcessSpreadsheetResult> Process(Guid importId, FileStream spreadsheet)
        {
            int totalTines = 1;
            int processed = 0;
            int errors = 0;
            var watch = Stopwatch.StartNew();

            using var reader = new StreamReader(spreadsheet);

            // pula cabeçalho

            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                Console.WriteLine(line);

                // Converte linha pra objeto 
                var importProduct = ParseLineToImportProduct(importId, line);

                // Grava na tabela de importação

                await _importProductRepository.Create(importProduct);

                // Cria mensagem para processamento do tópico

                await CreateMessage(importProduct);

                totalTines++;
            }

            // Envia mensagens para o tópico

            SendMessages();

            watch.Stop();
            var elapsedMs = watch.Elapsed;

            var result = new ProcessSpreadsheetResult
            {
                TotalLines = totalTines,
                Processed = processed,
                Errors = errors,
                ProcessTime = elapsedMs.ToString()
            };

            return result;
        }

        private static ImportProduct ParseLineToImportProduct(Guid importId, string line)
        {
            var values = line.Split(';');

            var importProduct = new ImportProduct(
                importId: importId,
                storeId: Convert.ToInt64(values[0]),
                productCode: values[1],
                name: values[3],
                price: Convert.ToDecimal(values[9]),
                stock: Convert.ToDecimal(values[14])
                );

            return importProduct;
        }

        private async Task CreateMessage(ImportProduct importProduct)
        {
            var json = JsonSerializer.Serialize(importProduct);

            var message = new Message<Null, string>
            {
                Value = json
            };

            await _producer.ProduceAsync("process-product-requested", message);
        }

        private void SendMessages()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}
