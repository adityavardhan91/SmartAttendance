using Microsoft.EntityFrameworkCore;
using SmartAttendance.IRepositories.AttendanceInterfaces;
using SmartAttendance.Models;

namespace SmartAttendance.Repository
{
    public class AttendanceRepository : IAttendanceViewRepository, IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;
        public AttendanceRepository(ApplicationDbContext context)
        {

            _context = context;

        }
        public async Task<Attendance> AddAttendance(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<Attendance> GetAttendance(string id)
        {
            var attendance = await _context.Attendances.FirstOrDefaultAsync(x => x.AttendanceId == id);
            return attendance;
        }

        public async Task<List<Attendance>> GetAttendances()
        {
            return _context.Attendances.ToList();
        }

        public async Task<Attendance> UpdateAttendance(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }
    }
}
