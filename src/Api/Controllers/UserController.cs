using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;
using Application.Requests;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserRequest request)
        {
            var userId = await _userService.Add(request);
            var res = new { Id = userId };
            return CreatedAtAction(nameof(GetById), new { id = userId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            await _userService.Update(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return NoContent();
        }
    }
}