using IdettaTestServer.DAL.DomainClasses;
using static IdettaTestServer.DAL.DomainClasses.ScheduleTask;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdettaTestServer.DAL.Helpers
{
    public class ScheduleTaskHelper
    {
        public string? StudentEmail { get; set; }
        public string? InstructorEmail { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public string? StudentComments { get; set; }
        public float Rating { get; set; }
        public DateTime? DateAssigned { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ScheduledDate { get; set; } = null;
        public Status TaskStatus { get; set; }
        public int Duration { get; set; }
    }
}
