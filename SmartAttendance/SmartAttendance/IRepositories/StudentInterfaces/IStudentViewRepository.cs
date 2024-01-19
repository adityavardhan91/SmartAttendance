using SmartAttendance.Models;

namespace SmartAttendance.IRepositories.StudentInterfaces
{
    public interface IStudentViewRepository
    {
        Task<List<Student>> GetAllStudents();
        Task<Student> GetStudentById(string studentId);
    }
}
