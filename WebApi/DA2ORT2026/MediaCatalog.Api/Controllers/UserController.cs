using MediaCatalog.Api.Filters;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediaCatalog.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Get()
        {
            List<UserDetailDTO> users = _userService.GetUsers();
            return Ok(new { users });
        }

        [HttpGet("by-email/{email}")]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult GetByEmail(string email)
        {
            UserDetailDTO user = _userService.GetUser(email);
            return Ok(new { user });
        }

        [HttpPost]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Create([FromBody] UserCreateDTO newUser)
        {
            UserDetailDTO user = _userService.AddUser(newUser);
            return Ok(new { user });
        }

        [HttpPut("{userId}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Update(int userId, [FromBody] UserUpdateDTO userToUpdate)
        {
            UserDetailDTO user = _userService.UpdateUserById(userId, userToUpdate);
            return Ok(new { user });
        }

        [HttpPut("by-email/{email}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult UpdateByEmail(string email, [FromBody] UserUpdateDTO userToUpdate)
        {
            UserDetailDTO user = _userService.UpdateUser(email, userToUpdate);
            return Ok(new { user });
        }

        [HttpDelete("by-email/{email}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult DeleteByEmail(string email)
        {
            _userService.DeleteUser(email);
            return Ok(new { Message = "User deleted successfully." });
        }

        [HttpDelete("{userId:int}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Delete(int userId)
        {
            _userService.DeleteUserById(userId);
            return Ok(new { Message = "User deleted successfully." });
        }

        [HttpPut("by-email/{email}/password")]
        [AuthorizationFilter("Administrator")]
        public IActionResult ChangePassword(string email, [FromBody] ChangePasswordDTO changePasswordDTO)
        {
            _userService.ChangePassword(email, changePasswordDTO);
            return Ok(new { Message = "Password updated successfully." });
        }
    }
}
