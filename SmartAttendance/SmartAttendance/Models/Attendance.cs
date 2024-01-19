using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SmartAttendance.Models
{
    public class Attendance
    {
        public string AttendanceId { get; set; }
        public string StudentId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
