using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProductsImport.ProductUpdater.Consumer.Domain.Imports.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Infrastructure.Repositories
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

        public async Task MarkProductAsProcessed(Guid importId, string productCode, string observation = null)
        {
            var sql = @"UPDATE ""ImportProducts""
                           SET ""IsProcessed""=true, ""Observation""=:Observation, ""ProcessedAt""=:ProcessedAt
                         WHERE ""ImportId""=:ImportId AND ""ProductCode""=:ProductCode;";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, new
            {
                Observation = observation,
                ProcessedAt = DateTime.Now,
                ImportId = importId,
                ProductCode = productCode
            });
        }

        public async Task<int> TotalProductsProcessing(Guid importId)
        {
            var sql = @"SELECT count(""ImportId"") 
                          FROM ""ImportProducts"" 
                         WHERE ""ImportId"" = :ImportId
                           AND ""IsProcessed"" = false";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.ExecuteScalarAsync<int>(sql, new { ImportId = importId });
        }
    }
}
