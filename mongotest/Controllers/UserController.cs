using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mongoAPI.Services;
using mongoAPI.Models;

namespace mongotest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService) => _userService = userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Password = userDTO.Password
            };

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(CreateUser), new { id = user.UserId }, user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers() =>
            Ok(await _userService.GetUserAsync());
    }
}
