using System.ComponentModel.DataAnnotations.Schema;


namespace IdettaTestServer.DAL.DomainClasses
{
    public class ActivityDetails
    {
        [ForeignKey("ActivityId")]
        public int ActivityId { get; set; }
        public Activity? Activity { get; set; }
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        public Person? Student { get; set; }
        [ForeignKey("InstructorId")]
        public int InstructorId { get; set; }   
        public string? Details { get; set; } //JSON containing activity parameters
    }
}
