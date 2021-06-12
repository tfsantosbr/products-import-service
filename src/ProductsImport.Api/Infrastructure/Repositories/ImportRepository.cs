using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProductsImport.Api.Domain.Imports;
using ProductsImport.Api.Domain.Imports.Models;
using ProductsImport.Api.Domain.Imports.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsImport.Api.Infrastructure.Repositories
{
    public class ImportRepository : IImportRepository
    {
        // Fields

        private readonly string _connectionString;

        // Constructors

        public ImportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        // Public Methods
        public async Task Create(Import import)
        {
            var sql = @"INSERT INTO ""Imports""
                                   (""Id""
                                   ,""CreatedAt""
                                   ,""CompletedAt""
                                   ,""SpreadsheetFileUrl"")
                            VALUES(:Id
                                   ,:CreatedAt
                                   ,:CompletedAt
                                   ,:SpreadsheetFileUrl)";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, import);
        }

        public async Task<ImportDetails> GetImportDetails(Guid importId)
        {
            var sql = @"SELECT ""Id""
	                          ,""CreatedAt""
	                          ,""CompletedAt""
	                          ,""SpreadsheetFileUrl""
	                          ,(select count(*) from ""ImportProducts"" ip where ip.""ImportId"" = ""Id"") as ""TotalItems""
	                          ,(select count(*) from ""ImportProducts"" ip where ip.""ImportId"" = ""Id"" and ip.""IsProcessed"" = true) as ""TotalItemsProcessed""
	                          ,(select count(*) from ""ImportProducts"" ip where ip.""ImportId"" = ""Id"" and ip.""IsProcessed"" = true AND ""Observation"" NOTNULL) as ""TotalErrors""
                          FROM ""Imports""
                         WHERE ""Id"" = :Id";

            using var connection = new NpgsqlConnection(_connectionString);
            var results = await connection.QueryFirstOrDefaultAsync<ImportDetails>(sql, new { Id = importId });

            return results;
        }

        public async Task<IEnumerable<ImportProductItem>> ListImportErrors(Guid importId)
        {
            var sql = @"SELECT ""ImportId"", ""ProductCode"", ""IsProcessed"", ""Observation"", ""ProcessedAt"", ""Line""
                          FROM ""ImportProducts""
                         WHERE  ""Observation"" NOTNULL AND ""ImportId"" = :ImportId";

            using var connection = new NpgsqlConnection(_connectionString);
            var results = await connection.QueryAsync<ImportProductItem>(sql, new { ImportId = importId });

            return results;
        }

        public async Task<IEnumerable<ImportItem>> ListImports()
        {
            var sql = @"SELECT ""Id""
	                          ,""CreatedAt""
	                          ,""CompletedAt""
	                          ,""SpreadsheetFileUrl""
	                          ,(select count(*) from ""ImportProducts"" ip where ip.""ImportId"" = ""Id"") as ""TotalItems""
	                          ,(select count(*) from ""ImportProducts"" ip where ip.""ImportId"" = ""Id"" and ip.""IsProcessed"" = true) as ""TotalItemsProcessed""
                        FROM ""Imports""
                    ORDER BY ""CreatedAt"" DESC
                       LIMIT 15";

            using var connection = new NpgsqlConnection(_connectionString);
            var results = await connection.QueryAsync<ImportItem>(sql);

            return results;
        }
    }
}
