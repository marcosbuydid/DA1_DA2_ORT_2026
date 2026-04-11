using MediaCatalog.Api.Filters;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
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
            return Ok(new ApiResponse<List<UserDetailDTO>> { Result = users });
        }

        [HttpGet("by-email/{email}")]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult GetByEmail(string email)
        {
            UserDetailDTO user = _userService.GetUser(email);
            return Ok(new ApiResponse<UserDetailDTO> { Result = user });
        }

        [HttpPost]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Create([FromBody] UserCreateDTO newUser)
        {
            UserDetailDTO user = _userService.AddUser(newUser);
            return Ok(new ApiResponse<UserDetailDTO> { Result = user });
        }

        [HttpPut("{userId}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Update(int userId, [FromBody] UserUpdateDTO userToUpdate)
        {
            UserDetailDTO user = _userService.UpdateUserById(userId, userToUpdate);
            return Ok(new ApiResponse<UserDetailDTO> { Result = user });
        }

        [HttpPut("by-email/{email}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult UpdateByEmail(string email, [FromBody] UserUpdateDTO userToUpdate)
        {
            UserDetailDTO user = _userService.UpdateUser(email, userToUpdate);
            return Ok(new ApiResponse<UserDetailDTO> { Result = user });
        }

        [HttpDelete("by-email/{email}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult DeleteByEmail(string email)
        {
            _userService.DeleteUser(email);
            string Message = "User deleted successfully.";
            return Ok(new ApiResponse<string> { Result = Message });
        }

        [HttpDelete("{userId:int}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Delete(int userId)
        {
            _userService.DeleteUserById(userId);
            string Message = "User deleted successfully.";
            return Ok(new ApiResponse<string> { Result = Message });
        }

        [HttpPut("by-email/{email}/password")]
        [AuthorizationFilter("Administrator")]
        public IActionResult ChangePassword(string email, [FromBody] ChangePasswordDTO changePasswordDTO)
        {
            _userService.ChangePassword(email, changePasswordDTO);
            string Message = "Password updated successfully.";
            return Ok(new ApiResponse<string> { Result = Message });
        }
    }
}
