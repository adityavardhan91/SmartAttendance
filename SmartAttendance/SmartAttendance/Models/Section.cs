namespace SmartAttendance.Models
{
    public class Section
    {
        public string SectionId {  get; set; }
        public string SectionName { get; set; }
        public List<Student> Students { get; set; }
    }
}
