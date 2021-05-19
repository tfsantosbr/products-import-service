using Microsoft.Extensions.Logging;
using ProductsImport.ProductUpdater.Consumer.Domain.Imports.Repositories;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Events;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Domain.Products.Handlers
{
    public class ProcessProductRequestedHandler : IProcessProductRequestedHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IImportRepository _importRepository;
        private readonly ILogger<ProcessProductRequestedHandler> _logger;

        public ProcessProductRequestedHandler(IProductRepository productRepository, IImportRepository importRepository, 
            ILogger<ProcessProductRequestedHandler> logger)
        {
            _productRepository = productRepository;
            _importRepository = importRepository;
            _logger = logger;
        }

        public async Task Handle(ProcessProductRequested notification)
        {
            var product = await _productRepository.GetProduct(notification.StoreId, notification.ProductCode);

            if (product is not null)
            {
                product.Update(
                    name: notification.Name,
                    price: notification.Price,
                    stock: notification.Stock
                    );

                await _productRepository.Update(product);

                _logger.LogInformation($"Product UPDATED: {product.StoreId} | {product.Code} | {product.ProcessedAt}");
            }
            else
            {
                product = new Product(
                    storeId: notification.StoreId,
                    code: notification.ProductCode,
                    name: notification.Name,
                    price: notification.Price,
                    stock: notification.Stock
                    );

                await _productRepository.Create(product);

                _logger.LogInformation($"Product CREATED: {product.StoreId} | {product.Code} | {product.ProcessedAt}");
            }

            // se for o último produto a ser atualizado, notifica finalização da importação
        }
    }
}
