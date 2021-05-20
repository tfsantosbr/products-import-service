using Microsoft.Extensions.Logging;
using ProductsImport.Notification.Consumer.Domain.Imports.Events;
using ProductsImport.Notification.Consumer.Domain.Imports.Repositories;
using ProductsImport.Notification.Consumer.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductsImport.Notification.Consumer.Domain.Imports.Handlers
{
    public class ImportCompletedHandler : IImportCompletedHandler
    {
        private readonly ILogger<ImportCompletedHandler> _logger;
        private readonly IImportRepository _importRepository;

        public ImportCompletedHandler(ILogger<ImportCompletedHandler> logger, IImportRepository importRepository)
        {
            _logger = logger;
            _importRepository = importRepository;
        }

        public async Task Handle(ImportCompleted notification)
        {
            await _importRepository.MarkImportCompleted(notification.ImportId, notification.CompletedAt);

            _logger.LogInformation($"IMPORT COMPLETED! - {notification.ImportId}");
        }
    }
}
