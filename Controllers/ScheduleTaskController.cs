using IdettaTestServer.DAL;
using IdettaTestServer.DAL.DAO;
using IdettaTestServer.DAL.DomainClasses;
using IdettaTestServer.DAL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdettaTestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleTaskController : ControllerBase
    {
        AppDbContext db;

        public ScheduleTaskController(AppDbContext db) 
        {
            this.db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ScheduleTaskHelper stHelper)
        {
            PersonDAO personDAO = new(db);
            Person? student = await personDAO.GetByEmail(stHelper.StudentEmail!);
            Person? instructor = await personDAO.GetByEmail(stHelper.InstructorEmail!);
            StudentInstructor? st = await db.StudentInstructors.FirstOrDefaultAsync(s => s.StudentId == student!.Id && s.InstructorId == instructor!.Id)!;
            if (student == null)
            {
                return BadRequest("Student email not found!");
            }

            if (instructor == null) 
            {
                return BadRequest("Instructor email not found");
            }
            ScheduleTask scheduleTask = new()
            {
                StudentTeacherId = st!.Id,
                Duration = stHelper.Duration,
                Name = stHelper.Title,
                Description = stHelper.Description,
                DueDate = stHelper.DueDate,
                ScheduledDate = stHelper.ScheduledDate,
                DateAssigned = stHelper.DateAssigned,
                LongDescription = stHelper.LongDescription,
                TaskStatus = stHelper.TaskStatus,
                StudentComments = stHelper.StudentComments,
                Rating = stHelper.Rating,

            };

            ScheduleTaskDAO dao = new(db);
            return Ok(await dao.Add(scheduleTask));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(ScheduleTaskHelper stHelper)
        {
            PersonDAO personDAO = new(db);
            Person? student = await personDAO.GetByEmail(stHelper.StudentEmail!);
            Person? instructor = await personDAO.GetByEmail(stHelper.InstructorEmail!);
            StudentInstructor? st = await db.StudentInstructors.FirstOrDefaultAsync(s => s.StudentId == student!.Id && s.InstructorId == instructor!.Id)!;
            if (student == null)
            {
                return BadRequest("Student email not found!");
            }

            if (instructor == null)
            {
                return BadRequest("Instructor email not found");
            }
            ScheduleTask scheduleTask = new()
            {
                StudentTeacherId = st!.Id,
                Duration = stHelper.Duration,
                Name = stHelper.Title,
                Description = stHelper.Description,
                DueDate = stHelper.DueDate,
                ScheduledDate = stHelper.ScheduledDate,
                DateAssigned = stHelper.DateAssigned,
                LongDescription = stHelper.LongDescription,
                TaskStatus = stHelper.TaskStatus,
                StudentComments = stHelper.StudentComments,
                Rating = stHelper.Rating,

            };

            ScheduleTaskDAO dao = new(db);
            return Ok(await dao.Update(scheduleTask));
        }

        [HttpGet]
        [Route("student/{studentId}")]
        public async Task<IActionResult> GetByStudentID(int studentId)
        {
            ScheduleTaskDAO dao = new(db);
            PersonDAO personDAO = new(db);
            List<ScheduleTask> st = await dao.GetScheduleTasksByStudentID(studentId);
            List<ScheduleTaskHelper> helper = new();
            Person? student = await personDAO.GetById(studentId);

            foreach (ScheduleTask task in st) 
            {

                StudentInstructor? studentTeacher = await db.StudentInstructors.FirstOrDefaultAsync(s => s.Id == task.StudentTeacherId)!;
                Person? instructor = await personDAO.GetById(studentTeacher!.InstructorId)!; 
                ScheduleTaskHelper currentHelper = new()
                {
                    StudentEmail = student!.EmailAddress,
                    InstructorEmail = instructor!.EmailAddress,
                    StudentComments = task.StudentComments,
                    Title = task.Name, 
                    Description = task.Description,
                    LongDescription= task.LongDescription,
                    TaskStatus = task.TaskStatus,
                    Rating= task.Rating,
                    DateAssigned = task.DateAssigned,
                    DueDate = task.DueDate,
                    ScheduledDate = task.ScheduledDate,
                    Duration = task.Duration,
                };

                helper.Add(currentHelper);
            }
            return Ok(helper);
        }

        [HttpGet]
        [Route("instructor/{instructorId}")]
        public async Task<IActionResult> GetByInstructorID(int instructorId)
        {
            ScheduleTaskDAO dao = new(db);
            return Ok(await dao.GetScheduleTasksByInstructorID(instructorId));
        }

        [HttpGet]
        [Route("instructor_student/{instructorId}/{studentId}")]
        public async Task<IActionResult> GetByInstructorAndStudentIds(int instructorId, int studentId)
        {
            ScheduleTaskDAO dao = new(db);
            return Ok(await dao.GetScheduleTasksByInstructorAndStudent(instructorId, studentId));
        }

    }
}
