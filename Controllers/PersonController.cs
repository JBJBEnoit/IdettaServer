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
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PersonController(AppDbContext db) 
        {
            _db = db;
        }

        [HttpGet]
        [Route("get_students/{instructorEmail}")]
        public async Task<List<Person>> GetStudents(string instructorEmail)
        {
            PersonDAO dao = new(_db);
            Person? instructor = await dao.GetByEmail(instructorEmail);
            return await dao.GetStudents(instructor!);
        }

        [HttpPost]
        [Route("new_activity")]
        [Produces("application/json")]
        public async Task<IActionResult> AddNewActivity(ActivityDetailsHelper activityDetails) 
        {
            try
            {
                ActivityDAO activityDAO = new ActivityDAO(_db);
                Activity? activity = await activityDAO.GetById((int)activityDetails.ActivityId!);
                PersonDAO personDAO = new PersonDAO(_db);
                Person? student = await personDAO.GetByEmail(activityDetails.StudentEmail!);
                Person? instructor = await personDAO.GetByEmail(activityDetails.InstructorEmail!);
                
                if (activity == null || student == null || instructor == null)
                {
                    throw new($"Credentials incomplete! activity: {activity}, student: {student}, instructor: {instructor}");
                }

                ActivityDetails details = new()
                {
                    ActivityId = activity.Id,
                    StudentId = student.Id,
                    InstructorId = instructor.Id,
                    Details = activityDetails.Details,
                };
                
                await _db.ActivityDetails.AddAsync(details);
                int res = await _db.SaveChangesAsync();

                if (res <= 0)
                {
                    throw new("add activity details returned zero");
                }

                res = await personDAO.Update(student);
                if (res <= 0) 
                {
                    throw new("Couldn't update student record");
                }
                return Ok("Activity added");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("all_activities/{student_id}")]
        public async Task<IActionResult> GetActivitiesByStudentId(int student_id)
        {
            try
            {
                List<ActivityPackage> activityPackages = new();

                List<ActivityDetails> activityDetails = await _db.ActivityDetails.Where(ad => ad.StudentId == student_id).ToListAsync();

                PersonDAO personDAO = new PersonDAO(_db);

                Person? student = await personDAO.GetById(student_id);

                foreach(ActivityDetails details in activityDetails) 
                {

                    Person? instructor = await personDAO.GetById(details.InstructorId);

                    
                    ActivityDetailsHelper adHelper = new()
                    {
                        ActivityId = details.ActivityId,
                        StudentEmail = student!.EmailAddress,
                        InstructorEmail = instructor!.EmailAddress,
                        Details = details.Details,
                    };

                    ActivityDAO actDAO = new(_db);

                    Activity? activity = await actDAO.GetById(details.ActivityId)!;

                    ActivityHelper activityHelper = new()
                    {
                        Func = activity?.Func,
                        Id = activity?.Id,
                        Name = activity?.Name,
                    };

                    ActivityPackage package = new()
                    {
                        Activity = activityHelper,
                        Details = adHelper
                    };

                    activityPackages.Add(package);  
                }

                return Ok(activityPackages);
            }
            catch (Exception ex) 
            {
                return BadRequest(BadRequest(ex.Message));
            }
        }

        /** NOTE This method is only for testing, and will be replaced with a
         *  registration method once authentication/authorization is implemented */
        [HttpPost]
        [Route("new_person")]
        [Produces("application/json")]
        public async Task<int> AddPerson(PersonHelper newPerson) 
        {

            try
            {
                Person person = new Person();
                person.FirstName = newPerson.FirstName;
                person.LastName = newPerson.LastName;
                person.EmailAddress = newPerson.EmailAddress;

                PersonDAO personDAO = new PersonDAO(_db);

                return await personDAO.Add(person);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        [HttpPost]
        [Route("update_activity")]
        [Produces("appliction/json")]
        public async Task<IActionResult> UpdateActivity(ActivityDetailsHelper activityDetailsHelper)
        {
            try 
            {
                int studentId, instructorId;
                PersonDAO dao = new(_db);
                Person? student = await dao.GetByEmail(activityDetailsHelper.StudentEmail!);
                Person? instructor = await dao.GetByEmail(activityDetailsHelper.InstructorEmail!);


                studentId = student!.Id;
                instructorId = instructor!.Id;
                ActivityDetails activityDetails = new() 
                {
                    StudentId = studentId,
                    InstructorId = instructorId,
                    ActivityId = (int)activityDetailsHelper.ActivityId!,
                    Details = activityDetailsHelper.Details
                };
                _db.ActivityDetails.Update(activityDetails);
                int numUpdates = await _db.SaveChangesAsync();

                if (numUpdates > 0) 
                {
                    return Ok("Activity updated");
                }
                else
                {
                    return BadRequest("Error: could not update activity");
                }

                
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("add_student")]
        [Produces("application/json")]
        public async Task<IActionResult> AddStudent(AddStudentHelper helper)
        {
            PersonDAO dao = new(_db);
            Person? instructor = await dao.GetByEmail(helper.InstructorEmail!);
            Person? student = await dao.GetByEmail(helper.StudentEmail!);
            
            return Ok(await dao.AddStudent(instructor!, student!));
        }

        [HttpPost]
        [Route("update_person")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdatePerson(PersonHelper updatedPerson) 
        {
            
            PersonDAO dao = new PersonDAO(_db);
            Person? person = await dao.GetByEmail(updatedPerson.EmailAddress!);

            if (person == null) 
            {
                return BadRequest("Email does not belong to a registered user");
            }
           
            person.FirstName = updatedPerson.FirstName;
            person.LastName = updatedPerson.LastName;
            int result = await dao.Update(person);
            if (result > 0) 
            {
                return Ok($"{person.FirstName} {person.LastName} updated");
            }

            return BadRequest("Could not complete update");  
        }

        private class ActivityPackage
        {
            public ActivityHelper? Activity { get; set; }
            public ActivityDetailsHelper? Details { get; set; }
            //public ActivityDetails? Details { get; set; }

        }
    }
}
