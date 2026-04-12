using Microsoft.Data.Sqlite;
using WebApp.Models;

namespace WebApp.Data
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository()
        {
            _connectionString = DatabaseInitializer.GetConnectionString();
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT EmployeeId, Name, Department, Email, Phone, CreatedAt FROM Employees ORDER BY Name", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Department = reader["Department"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                Phone = reader["Phone"].ToString() ?? string.Empty,
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            });
                        }
                    }
                }
            }

            return employees;
        }

        public Employee? GetEmployeeById(int employeeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT EmployeeId, Name, Department, Email, Phone, CreatedAt FROM Employees WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Department = reader["Department"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                Phone = reader["Phone"].ToString() ?? string.Empty,
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int AddEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "INSERT INTO Employees (Name, Department, Email, Phone) VALUES (@Name, @Department, @Email, @Phone); SELECT last_insert_rowid();", connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);

                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "UPDATE Employees SET Name = @Name, Department = @Department, Email = @Email, Phone = @Phone WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteEmployee(int employeeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("DELETE FROM Employees WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Employee> SearchEmployees(string searchTerm)
        {
            var employees = new List<Employee>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "SELECT EmployeeId, Name, Department, Email, Phone, CreatedAt FROM Employees WHERE Name LIKE @SearchTerm OR Email LIKE @SearchTerm OR Phone LIKE @SearchTerm OR Department LIKE @SearchTerm ORDER BY Name", connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Department = reader["Department"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                Phone = reader["Phone"].ToString() ?? string.Empty,
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            });
                        }
                    }
                }
            }

            return employees;
        }
    }
}
