using IdettaTestServer.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace IdettaTestServer.DAL.DAO
{
    public class ScheduleTaskDAO
    {
        AppDbContext db;
        public ScheduleTaskDAO(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Add(ScheduleTask scheduleTask)
        {
            db.ScheduleTasks.Add(scheduleTask);
            await db.SaveChangesAsync();

            return scheduleTask.Id;
        }

        public async Task<List<ScheduleTask>> GetScheduleTasksByStudentID(int studentID)
        {
            List<StudentInstructor> st = await db.StudentInstructors.Where(x => x.StudentId == studentID).ToListAsync();
            List<ScheduleTask> scheduleTasks = new List<ScheduleTask>();
            foreach (StudentInstructor student in st) 
            {
                ScheduleTask? scheduleTask = await db.ScheduleTasks.FirstOrDefaultAsync(x => x.StudentTeacherId == student.Id);
                scheduleTasks.Add(scheduleTask!);
            }
            return scheduleTasks;
        }

        public async Task<List<ScheduleTask>> GetScheduleTasksByInstructorID(int instructorID)
        {
            List<StudentInstructor> st = await db.StudentInstructors.Where(x => x.InstructorId == instructorID).ToListAsync();
            List<ScheduleTask> scheduleTasks = new List<ScheduleTask>();
            foreach (StudentInstructor studentTeacher in st)
            {
                ScheduleTask? scheduleTask = await db.ScheduleTasks.FirstOrDefaultAsync(x => x.StudentTeacherId == studentTeacher.Id);
                scheduleTasks.Add(scheduleTask!);
            }
            return scheduleTasks;
        }

        public async Task<List<ScheduleTask>> GetScheduleTasksByInstructorAndStudent(int instructorID, int studentID)
        {
            List<StudentInstructor> st = await db.StudentInstructors.Where(x => x.StudentId == studentID && x.InstructorId == instructorID).ToListAsync();
            List<ScheduleTask> scheduleTasks = new List<ScheduleTask>();
            foreach (StudentInstructor studentTeacher in st)
            {
                ScheduleTask? scheduleTask = await db.ScheduleTasks.FirstOrDefaultAsync(x => x.StudentTeacherId == studentTeacher.Id);
                scheduleTasks.Add(scheduleTask!);
            }
            return scheduleTasks;
        }

        public async Task<int> Update(ScheduleTask scheduleTask)
        {
            db.ScheduleTasks.Update(scheduleTask);
            return await db.SaveChangesAsync();
        }
    }
}
