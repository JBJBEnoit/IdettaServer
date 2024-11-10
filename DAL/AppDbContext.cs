using IdettaTestServer.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace IdettaTestServer.DAL
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityDetails>().HasKey(ap => new { ap.ActivityId, ap.StudentId, ap.InstructorId });
            modelBuilder.Entity<ScheduleTask>().Property(st => st.TaskStatus).HasConversion<int>();
            modelBuilder.Entity<Person>()
            .HasMany(e => e.Students)
            .WithMany(e => e.Instructors)
            .UsingEntity<StudentInstructor>(
                e => e.HasOne<Person>().WithMany().HasForeignKey(e => e.StudentId),
                e => e.HasOne<Person>().WithMany().HasForeignKey(e => e.InstructorId));


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());

                entityType.GetForeignKeys().Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.NoAction);
            }
        }
        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<ActivityDetails> ActivityDetails { get; set; }
        public virtual DbSet<ScheduleTask> ScheduleTasks { get; set;}
        public virtual DbSet<StudentInstructor> StudentInstructors { get; set; }
    }
}
