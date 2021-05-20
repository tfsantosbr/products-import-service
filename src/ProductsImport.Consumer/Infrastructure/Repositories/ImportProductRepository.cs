using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProductsImport.Consumer.Domain.Imports;
using ProductsImport.Consumer.Domain.Imports.Repositories;
using System.Threading.Tasks;

namespace ProductsImport.Consumer.Infrastructure.Repositories
{
    public class ImportProductRepository : IImportProductRepository
    {
        // Fields

        private readonly string _connectionString;

        // Constructors

        public ImportProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        // Public Methods

        public async Task Create(ImportProduct importProduct)
        {
            var sql = @"INSERT INTO ""imports-products""
                                   (""import-id""
                                   ,""product-code""
                                   ,""is-processed""
                                   ,""processed-at"")
                            VALUES (:ImportId
                                   ,:ProductCode
                                   ,false
                                   ,NULL)";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, importProduct);
        }
    }
}
