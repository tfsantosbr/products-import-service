using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProductsImport.Api.Domain.Imports;
using ProductsImport.Api.Domain.Imports.Repository;
using System;
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
    }
}
