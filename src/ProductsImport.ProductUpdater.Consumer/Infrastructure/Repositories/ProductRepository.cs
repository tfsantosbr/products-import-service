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
            var sql = @"INSERT INTO products (id, ""store-id"", code, ""name"", price, stock, ""processed-at"") 
                             VALUES (:Id, :StoreId, :Code, :Name, :Price, :Stock, :ProcessedAt);";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, product);
        }

        public async Task<Product> GetProduct(long storeId, string code)
        {
            var sql = @"SELECT id as ""Id""
                             , ""store-id"" as ""StoreId""
                             , code as ""Code""
                             , ""name"" as ""Name""
                             , price as ""Price""
                             , stock as ""Stock""
                             , ""processed-at"" as ""ProcessedAt""
                          FROM products
                         WHERE ""store-id"" = :StoreId 
                           AND code = :Code;";

            using var connection = new NpgsqlConnection(_connectionString);

            var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { StoreId = storeId, Code = code });

            return product;
        }

        public async Task Update(Product product)
        {
            var sql = @"UPDATE products 
                           SET ""store-id""=:StoreId
                             , code=:Code
                             , ""name""=:Name
                             , price=:Price
                             , stock=:Stock
                             , ""processed-at""=:ProcessedAt
                         WHERE id=:Id;";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(sql, product);
        }
    }
}
