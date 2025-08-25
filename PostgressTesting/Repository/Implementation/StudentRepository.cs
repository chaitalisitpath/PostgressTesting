//using System.Data;
//using Dapper;
//using Microsoft.EntityFrameworkCore;
//using Npgsql;
//using PostgressTesting.Models;
//using PostgressTesting.Repository.Abstraction;
//namespace PostgressTesting.Repository.Implementation
//{
//    public class StudentRepository : IStudent
//    {
//        private readonly PostGressSqlContext _context;
//        public StudentRepository(PostGressSqlContext context)
//        {
//            _context = context;
//        }
//        //public Student GetStudentById(int id)
//        //{
//        //    using (var connection = _context.Database.GetDbConnection())
//        //    {
//        //        if (connection.State == ConnectionState.Closed)
//        //            connection.Open();

//        //        return connection.QueryFirstOrDefault<Student>(
//        //            "sp_GetStudentById",
//        //            new { Id = id },
//        //            commandType: CommandType.StoredProcedure
//        //        );
//        //    }
//        //}

//        //public Student GetStudentById(int id)
//        //{
//        //    using (var connection = _context.Database.GetDbConnection())
//        //    {
//        //        if (connection.State == ConnectionState.Closed)
//        //            connection.Open();

//        //        // ✅ Call function using SELECT
//        //        return connection.QueryFirstOrDefault<Student>(
//        //            "SELECT * FROM sp_getstudentbyid(@p_id);",
//        //            new { p_id = id }   // match function parameter
//        //        );
//        //    }




//        //public void UpdateStudent(Student student)
//        //{
//        //    using (var connection = _context.Database.GetDbConnection())
//        //    {
//        //        if (connection.State == ConnectionState.Closed)
//        //            connection.Open();

//        //        connection.Execute(
//        //            "sp_UpdateStudent",
//        //            new
//        //            {
//        //                StudentId = student.StudentId,
//        //                FirstName = student.FirstName,
//        //                LastName = student.LastName,
//        //                Email = student.Email,
//        //                DateOfBirth = student.DateOfBirth,
//        //                EnrollmentDate = student.EnrollmentDate
//        //            },
//        //            commandType: CommandType.StoredProcedure
//        //        );
//        //    }
//        //}

//        //       public void UpdateStudent(Student student)
//        //       {
//        //           using (var connection = _context.Database.GetDbConnection())
//        //           {
//        //               if (connection.State == ConnectionState.Closed)
//        //                   connection.Open();

//        //               // ✅ PostgreSQL: use CALL for stored procedures
//        //               connection.Execute(
//        //    "CALL sp_updatestudent(@p_id, @p_firstname, @p_lastname, @p_email, @p_dateofbirth, @p_enrollmentdate);",
//        //    new
//        //    {
//        //        p_id = student.StudentId,
//        //        p_firstname = student.FirstName,
//        //        p_lastname = student.LastName,
//        //        p_email = student.Email,
//        //        p_dateofbirth = student.DateOfBirth,       // will map to TIMESTAMP
//        //        p_enrollmentdate = student.EnrollmentDate  // will map to TIMESTAMP
//        //    }
//        //);

//        //           }
//        //       }

//        public Student GetStudentById(int id)
//        {
//            using (var connection = _context.Database.GetDbConnection())
//            {
//                if (connection.State == ConnectionState.Closed)
//                    connection.Open();

//                // ✅ Call PostgreSQL function via SELECT
//                return connection.QueryFirstOrDefault<Student>(
//                    "SELECT * FROM sp_getstudentbyid(@p_id);",
//                    new { p_id = id },
//                    commandType: CommandType.Text
//                );
//            }
//        }

//        public void UpdateStudent(Student student)
//        {
//            using (var connection = _context.Database.GetDbConnection())
//            {
//                if (connection.State == ConnectionState.Closed)
//                    connection.Open();

//                // ✅ PostgreSQL: use CALL for stored procedures
//                connection.Execute(
//                    "CALL sp_updatestudent(@p_id, @p_firstname, @p_lastname, @p_email, @p_dateofbirth, @p_enrollmentdate);",
//                    new
//                    {
//                        p_id = student.StudentId,
//                        p_firstname = student.FirstName,
//                        p_lastname = student.LastName,
//                        p_email = student.Email,
//                        p_dateofbirth = student.DateOfBirth,       // maps to TIMESTAMP
//                        p_enrollmentdate = student.EnrollmentDate  // maps to TIMESTAMP
//                    },
//                    commandType: CommandType.Text
//                );
//            }
//        }


//    }
//}
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
