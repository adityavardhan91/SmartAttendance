using SmartAttendance.Models;

namespace SmartAttendance.IRepositories.AttendanceInterfaces
{
    public interface IAttendanceRepository
    {
        Task<Attendance> AddAttendance(Attendance attendance);
        Task<Attendance> UpdateAttendance(Attendance attendance);
    }
}
