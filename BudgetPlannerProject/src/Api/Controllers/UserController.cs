using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {
        private IUserService userService;
        public userController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var user = await userService.GetById(id);
            return Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userzes = await userService.GetAll();
            return Ok(userzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserDto user)
        {
            await userService.Add(user);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDto user)
        {
            var result = await userService.Update(user);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await userService.Delete(id);
            return Ok(result);
        }
    }
}


