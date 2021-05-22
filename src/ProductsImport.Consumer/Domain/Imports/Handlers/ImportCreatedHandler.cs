using Microsoft.Extensions.Logging;
using ProductsImport.Consumer.Domain.Imports.Events;
using ProductsImport.Consumer.Domain.Imports.Models;
using ProductsImport.Consumer.Domain.Imports.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProductsImport.Consumer.Domain.Imports.Handlers
{
    public class ImportCreatedHandler : IImportCreatedHandler
    {
        private readonly ILogger<ImportCreatedHandler> _logger;
        private readonly ISpreadsheetService _spreadsheetService;

        public ImportCreatedHandler(ILogger<ImportCreatedHandler> logger, ISpreadsheetService spreadsheetService)
        {
            _logger = logger;
            _spreadsheetService = spreadsheetService;
        }

        public async Task Handle(ImportCreated notification)
        {
            _logger.LogInformation($"Evento recebido: {notification.Id}, {notification.CreatedAt}, {notification.SpreadsheetFileUrl}");

            // Processa a Planilha

            var result = await ProcessSpreadsheet(notification.Id, notification.SpreadsheetFileUrl);
        }

        private async Task<ProcessSpreadsheetResult> ProcessSpreadsheet(Guid importId, string spreadsheetPath)
        {
            return await _spreadsheetService.Process(importId, spreadsheetPath);
        }
    }
}
