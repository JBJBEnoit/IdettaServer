using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdettaTestServer.DAL.DomainClasses
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }


        //[Required]
        //public string? Hash { get; set; }

        //[Required]
        //public string? Salt { get; set; }
        public virtual ICollection<ActivityDetails>? ActivityDetails { get; set; }
        public virtual ICollection<Person>? Students { get; set; }
        public virtual ICollection<Person>? Instructors { get; set; }
        //public virtual ICollection<ScheduleTask>? ScheduleTasks { get; set; }

        //public void AddNewActivity(Activity activity, int level)
        //{
        //    ActivityDetails details = new ActivityDetails();
        //    details.Activity = activity;
        //    details.Level = level;
        //    ActivityDetails?.Add(details);   
        //}
    }
}
