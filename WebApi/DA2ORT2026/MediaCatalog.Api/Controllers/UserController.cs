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

        [HttpPost]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Create([FromBody] UserCreateDTO newUser)
        {
            UserDetailDTO createdUser = _userService.AddUser(newUser);
            return Ok(createdUser);
        }
    }
}
