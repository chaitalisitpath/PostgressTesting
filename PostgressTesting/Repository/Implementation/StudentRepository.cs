using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PostgressTesting.Models;
using PostgressTesting.Repository.Abstraction;
namespace PostgressTesting.Repository.Implementation
{
    public class StudentRepository : IStudent
    {
        private readonly PostGressSqlContext _context;
        public StudentRepository(PostGressSqlContext context)
        {
            _context = context;
        }
        public Student GetStudentById(int id)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return connection.QueryFirstOrDefault<Student>(
                    "sp_GetStudentById",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                connection.Execute(
                    "sp_UpdateStudent",
                    new
                    {
                        StudentId = student.StudentId,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        DateOfBirth = student.DateOfBirth,
                        EnrollmentDate = student.EnrollmentDate
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

    }
}
