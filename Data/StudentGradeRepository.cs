using Microsoft.Data.Sqlite;
using WebApp.Models;

namespace WebApp.Data
{
    public class StudentGradeRepository
    {
        private readonly string _connectionString;

        public StudentGradeRepository()
        {
            _connectionString = DatabaseInitializer.GetConnectionString();
        }

        public List<StudentGrade> GetAllGrades()
        {
            var grades = new List<StudentGrade>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT GradeId, StudentName, Subject, Marks, CreatedAt FROM StudentGrades ORDER BY StudentName, Subject", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grades.Add(new StudentGrade
                            {
                                GradeId = Convert.ToInt32(reader["GradeId"]),
                                StudentName = reader["StudentName"].ToString() ?? string.Empty,
                                Subject = reader["Subject"].ToString() ?? string.Empty,
                                Marks = Convert.ToInt32(reader["Marks"]),
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            });
                        }
                    }
                }
            }

            return grades;
        }

        public List<StudentSummary> GetStudentSummary()
        {
            var summaries = new List<StudentSummary>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    @"SELECT StudentName, AVG(Marks) as AverageMarks, COUNT(*) as TotalSubjects 
                      FROM StudentGrades 
                      GROUP BY StudentName 
                      ORDER BY StudentName", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            summaries.Add(new StudentSummary
                            {
                                StudentName = reader["StudentName"].ToString() ?? string.Empty,
                                AverageMarks = Math.Round((double)reader["AverageMarks"], 2),
                                TotalSubjects = Convert.ToInt32(reader["TotalSubjects"])
                            });
                        }
                    }
                }
            }

            return summaries;
        }

        public StudentGrade? GetGradeById(int gradeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT GradeId, StudentName, Subject, Marks, CreatedAt FROM StudentGrades WHERE GradeId = @GradeId", connection))
                {
                    command.Parameters.AddWithValue("@GradeId", gradeId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentGrade
                            {
                                GradeId = Convert.ToInt32(reader["GradeId"]),
                                StudentName = reader["StudentName"].ToString() ?? string.Empty,
                                Subject = reader["Subject"].ToString() ?? string.Empty,
                                Marks = Convert.ToInt32(reader["Marks"]),
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString() ?? DateTime.Now.ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        public int AddGrade(StudentGrade grade)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "INSERT INTO StudentGrades (StudentName, Subject, Marks) VALUES (@StudentName, @Subject, @Marks); SELECT last_insert_rowid();", connection))
                {
                    command.Parameters.AddWithValue("@StudentName", grade.StudentName);
                    command.Parameters.AddWithValue("@Subject", grade.Subject);
                    command.Parameters.AddWithValue("@Marks", grade.Marks);

                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public bool UpdateGrade(StudentGrade grade)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(
                    "UPDATE StudentGrades SET StudentName = @StudentName, Subject = @Subject, Marks = @Marks WHERE GradeId = @GradeId", connection))
                {
                    command.Parameters.AddWithValue("@StudentName", grade.StudentName);
                    command.Parameters.AddWithValue("@Subject", grade.Subject);
                    command.Parameters.AddWithValue("@Marks", grade.Marks);
                    command.Parameters.AddWithValue("@GradeId", grade.GradeId);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteGrade(int gradeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("DELETE FROM StudentGrades WHERE GradeId = @GradeId", connection))
                {
                    command.Parameters.AddWithValue("@GradeId", gradeId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
