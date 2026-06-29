using MediaCatalog.Api.Filters;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
using Microsoft.AspNetCore.Mvc;

namespace MediaCatalog.Api.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginUserDTO loginUserDTO)
        {
            string token = _sessionService.Authenticate(loginUserDTO.Email, loginUserDTO.Password);

            return Ok(new ApiResponse<string> { Result = token });
        }

        [HttpGet]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult GetCurrentSession()
        {
            string? token = Request.Headers["Authorization"].FirstOrDefault()?
        .Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            SessionDTO? currentSession = _sessionService.ValidateSession(token);
            return Ok(new ApiResponse<SessionDTO> { Result = currentSession });
        }
    }
}
