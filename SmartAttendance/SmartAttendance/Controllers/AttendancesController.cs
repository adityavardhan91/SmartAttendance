using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartAttendance.IRepositories.AttendanceInterfaces;
using SmartAttendance.Models;

namespace SmartAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceViewRepository _attendanceViewRepository;
        public AttendancesController(IAttendanceRepository attendanceRepository, IAttendanceViewRepository attendanceViewRepository)
        {
            _attendanceRepository = attendanceRepository;

            _attendanceViewRepository = attendanceViewRepository;

        }
        [HttpGet("WholeAttendance")]
        public IActionResult GetAttendanceList()
        {
            try
            {
                return Ok(_attendanceViewRepository.GetAttendances());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("StudentAttendance")]
        public IActionResult GetAttendanceListByStudentId(string studentId) 
        {
            try
            {
                if (studentId == null)
                    return BadRequest();
                else
                {
                    var attendanceList = _attendanceViewRepository.GetAttendances();
                    var studentAttendance = attendanceList.Result.FindAll(x => x.StudentId == studentId);
                    return Ok(studentAttendance);
                }
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddStudentAttendance(string studentId)
        {
            try
            {
                if(studentId != null)
                {
                    var studentAttendanceexists = _attendanceViewRepository.GetAttendances()
                        .Result
                        .Where(x => x.StudentId == studentId)
                        .Where(x => x.DateTime == DateTime.Now);
                    if (studentAttendanceexists.Any())
                    {
                        return Ok("Attendance already added");
                    }
                    else
                    {
                        Attendance attendance = new Attendance()
                        {
                            StudentId = studentId,
                            DateTime = DateTime.Now,
                        };
                        _attendanceRepository.AddAttendance(attendance);
                        return Ok("Attendance added");
                    }
                }
                return BadRequest();
            }
            catch( Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
