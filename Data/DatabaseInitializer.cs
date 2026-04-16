using Microsoft.Data.Sqlite;

namespace WebApp.Data
{
    public class DatabaseInitializer
    {
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "app.db");
        private static readonly string ConnectionString = $"Data Source={DbPath};";

        public static void InitializeDatabase()
        {
            if (!File.Exists(DbPath))
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    CreateTables(connection);
                }
            }
            else
            {
                // Verify tables exist or create if missing
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    VerifyAndCreateTables(connection);
                }
            }
        }

        private static void CreateTables(SqliteConnection connection)
        {
            // Products table
            using (var command = new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS Products (
                    ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    Category TEXT NOT NULL,
                    Stock INTEGER NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );", connection))
            {
                command.ExecuteNonQuery();
            }

            // StudentGrades table
            using (var command = new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS StudentGrades (
                    GradeId INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentName TEXT NOT NULL,
                    Subject TEXT NOT NULL,
                    Marks INTEGER NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );", connection))
            {
                command.ExecuteNonQuery();
            }

            // Employees table
            using (var command = new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS Employees (
                    EmployeeId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Department TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Phone TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );", connection))
            {
                command.ExecuteNonQuery();
            }

            // Users table
            using (var command = new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void VerifyAndCreateTables(SqliteConnection connection)
        {
            // Check and create Products table if needed
            if (!TableExists(connection, "Products"))
            {
                using (var command = new SqliteCommand(@"
                    CREATE TABLE Products (
                        ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Price REAL NOT NULL,
                        Category TEXT NOT NULL,
                        Stock INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Check and create StudentGrades table if needed
            if (!TableExists(connection, "StudentGrades"))
            {
                using (var command = new SqliteCommand(@"
                    CREATE TABLE StudentGrades (
                        GradeId INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentName TEXT NOT NULL,
                        Subject TEXT NOT NULL,
                        Marks INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Check and create Employees table if needed
            if (!TableExists(connection, "Employees"))
            {
                using (var command = new SqliteCommand(@"
                    CREATE TABLE Employees (
                        EmployeeId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Department TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        Phone TEXT NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Check and create Users table if needed
            if (!TableExists(connection, "Users"))
            {
                using (var command = new SqliteCommand(@"
                    CREATE TABLE Users (
                        UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Email TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static bool TableExists(SqliteConnection connection, string tableName)
        {
            using (var command = new SqliteCommand(
                $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';", connection))
            {
                var result = command.ExecuteScalar();
                return result != null;
            }
        }

        public static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}

