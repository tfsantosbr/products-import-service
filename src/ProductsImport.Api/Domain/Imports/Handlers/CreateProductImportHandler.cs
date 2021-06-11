using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using ProductsImport.Api.Domain.Core.Services.Files;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Event;
using ProductsImport.Api.Domain.Imports.Models;
using ProductsImport.Api.Domain.Imports.Repository;

namespace ProductsImport.Api.Domain.Imports.Handlers
{
    public class CreateProductImportHandler : ICreateProductImportHandler
    {
        private readonly IFileService _fileService;
        private readonly IImportRepository _importRepository;
        private readonly IConfiguration _configuration;

        public CreateProductImportHandler(IFileService fileService, IImportRepository importRepository, IConfiguration configuration)
        {
            _fileService = fileService;
            _importRepository = importRepository;
            _configuration = configuration;
        }

        public async Task<ProductsImportResult> Handle(CreateProductsImport request)
        {
            // salva planilha no storage

            var fileId = Guid.NewGuid();
            var fileExtension = Path.GetExtension(request.FileName);
            var fileNewName = $"{fileId}{fileExtension}";
            var result = await _fileService.Upload(fileNewName, request.Data);

            // registra importação no banco de dados

            var import = new Import(result.FilePath);

            await _importRepository.Create(import);

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
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(importCreated) };

            var result = await producer.ProduceAsync("products-import-created", message);
        }
    }
}