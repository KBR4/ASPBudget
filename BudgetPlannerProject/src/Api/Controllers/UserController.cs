using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {
        private IUserService _userService;
        public userController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userzes = await _userService.GetAll();
            return Ok(userzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserDto user)
        {
            if (user == null)
            {
                return NotFound();
            }
            await _userService.Add(user);
            return Ok(user.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDto user)
        {
            var result = await _userService.Update(user);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}


