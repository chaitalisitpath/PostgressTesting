using System.Data;
using Microsoft.EntityFrameworkCore;
using PostgressTesting.Models;
using PostgressTesting.Repository.Abstraction;
using Dapper;
namespace PostgressTesting.Repository.Implementation
{
    public class StudentRepository : IStudent
    {
        private readonly PostGressSqlContext _context;
        public StudentRepository(PostGressSqlContext context)
        {
            _context = context;
        }
        public void UpdateStudent(Student student)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                connection.Execute("sp_UpdateStudent",
                new
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    DateOfBirth = student.DateOfBirth,
                    EnrollmentDate = student.EnrollmentDate
                },
                commandType: CommandType.StoredProcedure);


            }
        }
    }
}
