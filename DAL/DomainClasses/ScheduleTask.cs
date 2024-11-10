using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdettaTestServer.DAL.DomainClasses
{
    public class ScheduleTask
    {
        public enum Status
        {
            Pending, //student has not yet scheduled it
            Completed, // student has scheduled and completed it
            Overdue // student scheduled it but did not complete it
        }
        public int Id { get; set; }

        [ForeignKey("StudentTeacherId")]
        public int StudentTeacherId { get; set; }
        public virtual StudentInstructor? StudentTeacher { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public string? StudentComments { get; set; }
        public float Rating { get; set; }
        public string? SoundRecordingPath { get; set; }
        public DateTime? DateAssigned { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ScheduledDate { get; set; } = null;
        public Status TaskStatus { get; set; }
        public int Duration { get; set; } //how long the session should take, in minutes

    }
}
