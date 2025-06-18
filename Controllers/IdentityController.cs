using ApiMovie.Common;
using ApiMovie.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(JwtTokenGenerator jwtTokenGenerator) : ControllerBase
    {
        [HttpPost("sign-in")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username != "admin" || request.Password != "admin")
            {
                return BadRequest("Username atau password salah");
            }

            var token = jwtTokenGenerator.GenerateToken(request.Username, "Admin");

            return Ok(new { Token = token });
        }
    }
}
