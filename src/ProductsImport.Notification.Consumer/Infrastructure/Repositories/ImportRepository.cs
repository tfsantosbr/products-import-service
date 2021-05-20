using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProductsImport.Notification.Consumer.Domain.Imports.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.Notification.Consumer.Infrastructure.Repositories
{
    public class ImportRepository : IImportRepository
    {
        // Fields

        private readonly string _connectionString;

        // Constructors

        public ImportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ImportsDb");
        }

        // Public Methods

        public async Task MarkImportCompleted(Guid importId, DateTime completedAt)
        {
            var sql = @"UPDATE ""imports""
                           SET ""completed-at""=:CompletedAt
                         WHERE id=:Id";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, new
            {
                Id = importId,
                CompletedAt = completedAt
            });
        }
    }
}
