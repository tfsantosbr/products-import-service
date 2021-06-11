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
            var sql = @"INSERT INTO imports 
                                   (id
                                   ,""created-at""
                                   ,""completed-at""
                                   ,""spreadsheet-file-url"")
                            VALUES(:Id
                                   ,:CreatedAt
                                   ,:CompletedAt
                                   ,:SpreadsheetFileUrl)";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, import);
        }

        public async Task<IEnumerable<ImportItem>> ListImports()
        {
            var sql = @"SELECT id
	                          ,""created-at"" as CreatedAt
	                          ,""completed-at"" as CompletedAt
	                          ,""spreadsheet-file-url"" as SpreadsheetFileUrl
	                          ,(select count(*) from ""imports-products"" ip where ip.""import-id"" = id) as TotalItems
	                          ,(select count(*) from ""imports-products"" ip where ip.""import-id"" = id and ip.""is-processed"" = true) as TotalItemsProcessed
                        FROM imports
                    ORDER BY ""created-at"" DESC
                       LIMIT 15; ";

            using var connection = new NpgsqlConnection(_connectionString);
            var results = await connection.QueryAsync<ImportItem>(sql);

            return results;
        }
    }
}
