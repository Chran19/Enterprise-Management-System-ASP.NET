using Microsoft.Data.Sqlite;
using WebApp.Models;

namespace WebApp.Data
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository()
        {
            _connectionString = DatabaseInitializer.GetConnectionString();
        }

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT ProductId, Name, Price, Category, Stock, CreatedAt FROM Products ORDER BY CreatedAt DESC", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Price = (decimal)(double)reader["Price"],
                                Category = reader["Category"].ToString() ?? string.Empty,
                                Stock = Convert.ToInt32(reader["Stock"]),
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            });
                        }
                    }
                }
            }

            return products;
        }

        public Product? GetProductById(int productId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT ProductId, Name, Price, Category, Stock, CreatedAt FROM Products WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Price = (decimal)(double)reader["Price"],
                                Category = reader["Category"].ToString() ?? string.Empty,
                                Stock = Convert.ToInt32(reader["Stock"]),
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int AddProduct(Product product)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "INSERT INTO Products (Name, Price, Category, Stock) VALUES (@Name, @Price, @Category, @Stock); SELECT last_insert_rowid();", connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Category", product.Category);
                    command.Parameters.AddWithValue("@Stock", product.Stock);

                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public bool UpdateProduct(Product product)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "UPDATE Products SET Name = @Name, Price = @Price, Category = @Category, Stock = @Stock WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Category", product.Category);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteProduct(int productId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("DELETE FROM Products WHERE ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "SELECT ProductId, Name, Price, Category, Stock, CreatedAt FROM Products WHERE Name LIKE @SearchTerm OR Category LIKE @SearchTerm ORDER BY CreatedAt DESC", connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Price = (decimal)(double)reader["Price"],
                                Category = reader["Category"].ToString() ?? string.Empty,
                                Stock = Convert.ToInt32(reader["Stock"]),
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            });
                        }
                    }
                }
            }

            return products;
        }
    }
}
