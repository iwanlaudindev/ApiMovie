using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ApiMovie.Data;
using ApiMovie.Models;
using ApiMovie.Requests;
using ApiMovie.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMovie.Controllers
{
    [ApiVersion("1.0")] // API versi 1.0
    [ApiVersion("2.0")] // API versi 2.0
    [Route("api/v{version:apiVersion}/[controller]")] // Route API versi 1.0
    // [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    // [Authorize(Roles = "Admin")]
    public class StudentController(ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        // [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(ResponseList<StudentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudent(GetAllStudentRequest request)
        {
            var queryable = dbContext.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                queryable = queryable.Where(s => EF.Functions.ILike(s.Name, $"%{request.Search}%") ||
                    EF.Functions.ILike(s.Email, $"%{request.Search}%"));
            }

            var orderAbleColumn = GetAllStudentRequest.OrderAbleColumn
                .All(x => !string.Equals(request.OrderBy, x, StringComparison.OrdinalIgnoreCase));

            if (request.OrderBy != nameof(StudentResponse.Id) && orderAbleColumn)
            {
                request.OrderBy = nameof(StudentResponse.Id);
            }

            if (string.IsNullOrWhiteSpace(request.OrderType))
            {
                request.OrderType = "DESC";
            }

            var students = await queryable
                .OrderBy($"{request.OrderBy} {request.OrderType}")
                .Select(s => new StudentResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Age = s.Age
                })
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var responseList = new ResponseList<StudentResponse>();
            responseList.Pagination = new PaginationResponseWrapper
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalData = await queryable.CountAsync()
            };
            responseList.Ok(students);

            return Ok(responseList);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(ResponseList<StudentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentV2(GetAllStudentRequest request)
        {
            var queryable = dbContext.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                queryable = queryable.Where(s => EF.Functions.ILike(s.Name, $"%{request.Search}%") ||
                    EF.Functions.ILike(s.Email, $"%{request.Search}%"));
            }

            var orderAbleColumn = GetAllStudentRequest.OrderAbleColumn
                .All(x => !string.Equals(request.OrderBy, x, StringComparison.OrdinalIgnoreCase));

            if (request.OrderBy != nameof(StudentResponse.Id) && orderAbleColumn)
            {
                request.OrderBy = nameof(StudentResponse.Id);
            }

            if (string.IsNullOrWhiteSpace(request.OrderType))
            {
                request.OrderType = "DESC";
            }

            var students = await queryable
                .OrderBy($"{request.OrderBy} {request.OrderType}")
                .Select(s => new StudentResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Age = s.Age
                })
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var responseList = new ResponseList<StudentResponse>();
            responseList.Pagination = new PaginationResponseWrapper
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalData = await queryable.CountAsync()
            };
            responseList.Ok(students);

            return Ok(responseList);
        }

        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] CreateStudentRequest request)
        {
            var student = new Student
            {
                Name = request.Name!,
                Email = request.Email!,
                Age = request.Age
            };

            dbContext.Students.Add(student);
            await dbContext.SaveChangesAsync();

            return Ok(student);
        }

        [HttpGet("{id}/hobby")]
        public async Task<IActionResult> GetStudentById(StudentDetailRequest request)
        {
            var student = await dbContext.Students.SingleOrDefaultAsync(s => s.Id == request.Id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] UpdateStudentRequest request)
        {
            var student = await dbContext.Students.SingleOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            student.Name = request.Name!;
            student.Email = request.Email!;
            student.Age = request.Age;

            await dbContext.SaveChangesAsync();

            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // var student = await dbContext.Students.SingleOrDefaultAsync(s => s.Id == id);
            var student = await(
                from s in dbContext.Students
                join sa in dbContext.StudentAddreses on s.Id equals sa.StudentId into saGroup
                from sa in saGroup.DefaultIfEmpty()
                where s.Id == id
                select new Student
                {
                    Id = s.Id,
                    Name = s.Name,
                    StudentAddreses = saGroup.Select(x => new StudentAddres
                    {
                        Id = x.Id,
                        Street = x.Street,
                        StudentId = x.StudentId
                    }).ToList(),
                }
            ).SingleOrDefaultAsync();
            if (student == null)
            {
                return NotFound();
            }

            dbContext.Students.Remove(student);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
