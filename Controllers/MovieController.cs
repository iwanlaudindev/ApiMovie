using ApiMovie.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        public IActionResult GetStudent()
        {
            var students = new[] { "Alice", "Bob" };
            return Ok(students);
        }

        [HttpPost]
        public IActionResult PostStudent([FromBody] MovieRequest request)
        {
            return Ok(request);
        }
    }
}
