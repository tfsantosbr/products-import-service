using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProductsImport.ProductUpdater.Consumer.Domain.Products;
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // Fields

        private readonly string _connectionString;

        // Constructors

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProductsDb");
        }

        // Public Methods

        public async Task Create(Product product)
        {
            var sql = @"INSERT INTO ""Products"" (""Id"", ""StoreId"", ""Code"", ""Name"", ""Price"", ""Stock"", ""ProcessedAt"") 
                             VALUES (:Id, :StoreId, :Code, :Name, :Price, :Stock, :ProcessedAt);";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, product);
        }

        public async Task<Product> GetProduct(long storeId, string code)
        {
            var sql = @"SELECT ""Id""
                              ,""StoreId""
                              ,""Code""
                              ,""Name""
                              ,""Price""
                              ,""Stock""
                              ,""ProcessedAt""
                          FROM ""Products""
                         WHERE ""StoreId"" = :StoreId 
                           AND ""Code"" = :Code;";

            using var connection = new NpgsqlConnection(_connectionString);

            var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { StoreId = storeId, Code = code });

            return product;
        }

        public async Task Update(Product product)
        {
            var sql = @"UPDATE ""Products""
                           SET ""StoreId""=:StoreId
                             , ""Code""=:Code
                             , ""Name""=:Name
                             , ""Price""=:Price
                             , ""Stock""=:Stock
                             , ""ProcessedAt""=:ProcessedAt
                         WHERE ""Id""=:Id;";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, product);
        }
    }
}
