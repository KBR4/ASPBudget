using Application.Requests;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            var userId = await authService.Register(request);
            return CreatedAtAction(
               actionName: nameof(UserController.GetById),
               controllerName: "User",
               routeValues: new { id = userId },
               value: new { Id = userId });
        }

        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await authService.Login(request);
            return Ok(response);
        }
    }
}
