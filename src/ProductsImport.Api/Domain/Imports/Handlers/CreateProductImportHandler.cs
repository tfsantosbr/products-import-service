using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using ProductsImport.Api.Domain.Core.Services.Files;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Event;
using ProductsImport.Api.Domain.Imports.Models;
using ProductsImport.Api.Domain.Imports.Repository;

namespace ProductsImport.Api.Domain.Imports.Handlers
{
    public class CreateProductImportHandler : ICreateProductImportHandler
    {
        private readonly IFileService fileService;
        private readonly IImportRepository importRepository;

        public CreateProductImportHandler(IFileService fileService, IImportRepository importRepository)
        {
            this.fileService = fileService;
            this.importRepository = importRepository;
        }

        public async Task<ProductsImportResult> Handle(CreateProductsImport request)
        {
            // salva planilha no storage

            var result = await fileService.Upload(request.FileName, request.Data);

            // registra importação no banco de dados

            var import = new Import(result.FilePath);

            await importRepository.Create(import);

            // Envia mensagem para o tópico

            var importCreated = new ImportCreated
            {
                Id = import.Id,
                CreatedAt = import.CreatedAt,
                SpreadsheetFileUrl = import.SpreadsheetFileUrl
            };

            await SendEvent(importCreated);

            return new ProductsImportResult
            {
                ImportId = import.Id,
                FilePath = result.FilePath
            };
        }

        private async Task SendEvent(ImportCreated importCreated)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9091,localhost:9092,localhost:9093"
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var json = JsonSerializer.Serialize(importCreated);

            var message = new Message<Null, string>
            {
                Value = json
            };

            var result = await producer.ProduceAsync("products-import-created", message);
        }
    }
}