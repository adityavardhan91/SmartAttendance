using SmartAttendance.Models;

namespace SmartAttendance.IRepositories.IStudentRepo
{
    public interface IStudentManagementRepository
    {
        Task<Student> CreateStudent(Student student);
        Task<Student> UpdateStudentDetails(Student student);
    }
}
