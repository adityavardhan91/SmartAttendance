using SmartAttendance.Models;

namespace SmartAttendance.IRepositories.AttendanceInterfaces
{
    public interface IAttendanceViewRepository
    {
        Task<List<Attendance>> GetAttendances();
        Task<Attendance> GetAttendance(string id);
    }
}
