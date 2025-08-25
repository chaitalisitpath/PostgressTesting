using PostgressTesting.Models;

namespace PostgressTesting.Repository.Abstraction
{
    public interface IStudent
    {
        Student GetStudentById(int id);
        void UpdateStudent(Student student);
    }
}
                        