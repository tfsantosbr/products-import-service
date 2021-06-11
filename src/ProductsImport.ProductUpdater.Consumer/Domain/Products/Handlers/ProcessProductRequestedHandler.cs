using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductsImport.ProductUpdater.Consumer.Domain.Imports.Events;
using ProductsImport.ProductUpdater.Consumer.Domain.Imports.Repositories;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Events;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Repositories;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Domain.Products.Handlers
{
    public class ProcessProductRequestedHandler : IProcessProductRequestedHandler
    {
        // Fields

        private readonly IProductRepository _productRepository;
        private readonly IImportRepository _importRepository;
        private readonly ILogger<ProcessProductRequestedHandler> _logger;
        private readonly IConfiguration _configuration;

        // Constructors

        public ProcessProductRequestedHandler(IProductRepository productRepository, IImportRepository importRepository,
            ILogger<ProcessProductRequestedHandler> logger, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _importRepository = importRepository;
            _logger = logger;
            _configuration = configuration;
        }

        // Implementations

        public async Task Handle(ProcessProductRequested notification)
        {
            var product = await _productRepository.GetProduct(notification.StoreId, notification.ProductCode);

            if (product is null)
            {
                await CreateProduct(notification);
            }
            else
            {
                await UpdateProduct(product, notification);
            }

            // se for o último produto a ser atualizado, notifica finalização da importação
            if (await IsLastProductImported(notification.ImportId))
            {
                await RaiseEventImportCompleted(notification.ImportId);
            }
        }

        // Private Methods

        private async Task CreateProduct(ProcessProductRequested notification)
        {
            var product = new Product(
                    storeId: notification.StoreId,
                    code: notification.ProductCode,
                    name: notification.Name,
                    price: notification.Price,
                    stock: notification.Stock
                    );

            var error = ValidateProduct(product);

            if (string.IsNullOrWhiteSpace(error))
            {
                await _productRepository.Create(product);
            }

            _logger.LogInformation($"Product CREATED: {product.StoreId} | {product.Code} | {product.ProcessedAt}");

            await _importRepository.MarkProductAsProcessed(notification.ImportId, notification.ProductCode, error);
        }

        private async Task UpdateProduct(Product product, ProcessProductRequested notification)
        {
            product.Update(
                name: notification.Name,
                price: notification.Price,
                stock: notification.Stock
                );

            var error = ValidateProduct(product);

            if (string.IsNullOrWhiteSpace(error))
            {
                await _productRepository.Update(product);

                _logger.LogInformation($"Product UPDATED: {product.StoreId} | {product.Code} | {product.ProcessedAt}");
            }

            await _importRepository.MarkProductAsProcessed(notification.ImportId, notification.ProductCode, error);
        }

        private static string ValidateProduct(Product product)
        {
            if (product.Stock < 0)
            {
                return "Produto com estoque negativo";
            }

            if (product.Price == 0)
            {
                return "Produto com preço igual a 0";
            }

            return null;
        }

        private async Task<bool> IsLastProductImported(Guid importId)
        {
            return await _importRepository.TotalProductsProcessing(importId) == 0;
        }

        private async Task RaiseEventImportCompleted(Guid importId)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                EnableDeliveryReports = false
            };

            var producer = new ProducerBuilder<Null, string>(config).Build();

            var importCompleted = new ImportCompleted(importId);

            var json = JsonSerializer.Serialize(importCompleted);

            var message = new Message<Null, string>
            {
                Value = json
            };

            await producer.ProduceAsync("products-import-completed", message);
        }
    }
}
