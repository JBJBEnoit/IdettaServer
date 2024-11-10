using IdettaTestServer.DAL.DomainClasses;
using IdettaTestServer.DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace IdettaTestServer.DAL.DAO
{
    public class PersonDAO
    {
        AppDbContext _db;

        public PersonDAO(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<Person?> GetByEmail(string email)
        {

            Person? person = await _db.Persons.FirstOrDefaultAsync(x => x.EmailAddress == email);
            return person;
        }

        public async Task<Person?> GetById(int id)
        {
            Person? person = await _db.Persons.FirstOrDefaultAsync(_ => _.Id == id);
            return person;
        }

        public async Task<int> Update(Person person)
        {
            _db.Persons.Update(person);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> Add(Person person)
        {
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();

            return person.Id;   
        }

        public async Task<int> AddStudent(Person instructor, Person student)
        {
            var count = await _db.StudentInstructors.Where(st => st.InstructorId == instructor.Id && st.StudentId == student.Id).CountAsync();
            
            if (count == 0)
            {
                await _db.StudentInstructors.AddAsync(new StudentInstructor { InstructorId = instructor.Id, StudentId = student.Id });
            }

            return await _db.SaveChangesAsync();
        }

        public async Task<List<Person>> GetStudents(Person instructor)
        {
            var idList = await _db.StudentInstructors.Where(st => st.InstructorId == instructor.Id).ToListAsync();

            List<Person> students = new List<Person>();

            foreach (var studentTeacher in idList)
            {
                Person? student = await _db.Persons.FirstOrDefaultAsync(p => p.Id == studentTeacher.StudentId);

                if (student == null)
                {
                    continue;
                }

                students.Add(student);
            }

            return students;
        }
        
    }
}
