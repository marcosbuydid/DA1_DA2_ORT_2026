using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
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
            return new JsonResult(token)
            { StatusCode = StatusCodes.Status200OK };
        }
    }
}
