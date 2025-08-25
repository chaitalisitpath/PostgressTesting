using System.Data;
using Dapper;
using Npgsql;
using Microsoft.Extensions.Configuration;
using PostgressTesting.Models;
using PostgressTesting.Repository.Abstraction;

namespace PostgressTesting.Repository.Implementation
{
    public class StudentRepository : IStudent
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            // Read connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Student GetStudentById(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            return connection.QuerySingleOrDefault<Student>(
                "SELECT * FROM sp_getstudentbyid(@p_id);",
                new { p_id = id },
                commandType: CommandType.Text
            );
        }

        public void UpdateStudent(Student student)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            connection.Execute(
                "sp_updatestudent",
                new
                {
                    p_id = student.StudentId,
                    p_firstname = student.FirstName,
                    p_lastname = student.LastName,
                    p_email = student.Email,
                    p_dateofbirth = student.DateOfBirth,
                    p_enrollmentdate = student.EnrollmentDate
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
