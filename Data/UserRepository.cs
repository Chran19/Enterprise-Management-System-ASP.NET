using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using WebApp.Models;

namespace WebApp.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            _connectionString = DatabaseInitializer.GetConnectionString();
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT UserId, Username, Email, PasswordHash, CreatedAt FROM Users ORDER BY CreatedAt DESC", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                PasswordHash = reader["PasswordHash"].ToString() ?? string.Empty,
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            });
                        }
                    }
                }
            }

            return users;
        }

        public User? GetUserById(int userId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT UserId, Username, Email, PasswordHash, CreatedAt FROM Users WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                PasswordHash = reader["PasswordHash"].ToString() ?? string.Empty,
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        public User? GetUserByUsername(string username)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT UserId, Username, Email, PasswordHash, CreatedAt FROM Users WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                PasswordHash = reader["PasswordHash"].ToString() ?? string.Empty,
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int AddUser(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@Username, @Email, @PasswordHash); SELECT last_insert_rowid();", connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public bool UpdateUser(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "UPDATE Users SET Username = @Username, Email = @Email, PasswordHash = @PasswordHash WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@UserId", user.UserId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteUser(int userId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("DELETE FROM Users WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UsernameExists(string username)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    var result = command.ExecuteScalar();
                    return (long)(result ?? 0) > 0;
                }
            }
        }

        public bool EmailExists(string email)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    var result = command.ExecuteScalar();
                    return (long)(result ?? 0) > 0;
                }
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}
