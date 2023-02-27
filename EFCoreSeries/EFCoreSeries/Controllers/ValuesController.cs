using Entities;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSeries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ValuesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entity = _context.Model.FindEntityType(typeof(Student).FullName);

            var tableName = entity.GetTableName();
            var schemaName = entity.GetSchema();
            var key = entity.FindPrimaryKey();
            var properties = entity.GetProperties();

            var student = _context.Students.FirstOrDefault();
            _context.Entry(student)
                    .Collection(s => s.Evaluations)
                    .Load();

            _context.Entry(student)
                .Collection(s => s.StudentSubjects)
                .Load();

            _context.Entry(student)
                .Reference(s => s.StudentDetails)
                .Load();

            var evaluationsCount = _context.Entry(student)
                .Collection(s => s.Evaluations)
                .Query()
                .Count();

            var gradesPerStudent = _context.Entry(student)
                .Collection(s => s.Evaluations)
                .Query()
                .Select(e => e.Grade)
                .ToList();

            var studentForUpdate = _context.Students
                .FirstOrDefault(s => s.Name.Equals("Mike Miles"));
            var age = 28;

            var rowsAffected = _context.Database
                .ExecuteSqlRaw(@"UPDATE Student 
                       SET Age = {0} 
                       WHERE Name = {1}", age, studentForUpdate.Name);

            _context.Entry(studentForUpdate).Reload();

            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Student student)
        {
            //validation code goes here

            student.StudentDetails = new StudentDetails
            {
                Address = "Added Address",
                AdditionalInformation = "Additional information added"
            };

            _context.Add(student);
            _context.SaveChanges();

            return Created("URI of the created entity", student);
        }

        [HttpPut("{id}")]
        public IActionResult PUT(Guid id, [FromBody] Student student)
        {
            var dbStudent = _context.Students
                .FirstOrDefault(s => s.Id.Equals(id));

            dbStudent.Age = student.Age;
            dbStudent.Name = student.Name;
            dbStudent.IsRegularStudent = student.IsRegularStudent;

            var isAgeModified = _context.Entry(dbStudent).Property("Age").IsModified;
            var isNameModified = _context.Entry(dbStudent).Property("Name").IsModified;
            var isIsRegularStudentModified = _context.Entry(dbStudent).Property("IsRegularStudent").IsModified;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/relationship")]
        public IActionResult UpdateRelationship(Guid id, [FromBody] Student student)
        {
            var dbStudent = _context.Students
                .Include(s => s.StudentDetails)
                .FirstOrDefault(s => s.Id.Equals(id));

            dbStudent.Age = student.Age;
            dbStudent.Name = student.Name;
            dbStudent.IsRegularStudent = student.IsRegularStudent;
            dbStudent.StudentDetails.AdditionalInformation = "Additional information updated";

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/relationship2")]
        public IActionResult UpdateRelationship2(Guid id, [FromBody] Student student)
        {
            var dbStudent = _context.Students
                .FirstOrDefault(s => s.Id.Equals(id));

            dbStudent.StudentDetails = new StudentDetails
            {
                Id = new Guid("e2a3c45d-d19a-4603-b983-7f63e2b86f14"),
                Address = "added address",
                AdditionalInformation = "Additional information for student 2"
            };

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("disconnected")]
        public IActionResult UpdateDisconnected([FromBody] Student student)
        {
            _context.Students.Attach(student);
            _context.Entry(student).State = EntityState.Modified;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("disconnected2")]
        public IActionResult UpdateDisconnected2([FromBody] Student student)
        {
            _context.Update(student);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var student = _context.Students
                .FirstOrDefault(s => s.Id.Equals(id));

            if (student == null)
                return BadRequest();

            _context.Remove(student);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
