using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovie.Controllers
{
    // [ApiVersion("1.0")] // API versi 1.0
    // [Route("api/v{version:apiVersion}/[controller]")] // Route API versi 1.0
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    // [Authorize(Roles = "Admin")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudentV1()
        {
            var students = new[] { "Alice", "Bob" };
            return Ok(students);
        }
    }
}
