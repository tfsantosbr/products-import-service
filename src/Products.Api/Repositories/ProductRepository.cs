using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Products.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Api.Repositories
{
    public class ProductRepository
    {
        // Fields

        private readonly string _connectionString;

        // Constructors

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        // Public Methods

        public async Task<ProductDetails> GetDetails(int storeId, Guid productId)
        {
            var sql = @"SELECT ""Id"", ""StoreId"", ""Code"", ""Name"", ""Price"", ""Stock"", ""ProcessedAt"" 
                          FROM ""Products""
                         WHERE ""StoreId"" = :StoreId AND ""Id"" = :Id";

            using var connection = new NpgsqlConnection(_connectionString);
            var results = await connection.QueryFirstOrDefaultAsync<ProductDetails>(sql, new { StoreId = storeId, Id = productId });

            return results;
        }

        public async Task<IEnumerable<ProductItem>> ListProducts(int storeId)
        {
            var sql = @"SELECT ""Id"", ""StoreId"", ""Code"", ""Name""
                          FROM ""Products""
                         WHERE ""StoreId"" = :StoreId";

            using var connection = new NpgsqlConnection(_connectionString);
            var results = await connection.QueryAsync<ProductItem>(sql, new { StoreId = storeId });

            return results;
        }
    }
}
