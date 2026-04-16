using MediaCatalog.Api.Filters;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
using Microsoft.AspNetCore.Mvc;

namespace MediaCatalog.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Get()
        {
            List<RoleDetailDTO> roles = _roleService.GetRoles();
            return Ok(new ApiResponse<List<RoleDetailDTO>> { Result = roles });
        }

        [HttpGet("by-name/{name}")]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult GetByName(string name)
        {
            RoleDetailDTO role = _roleService.GetRole(name);
            return Ok(new ApiResponse<RoleDetailDTO> { Result = role });
        }

        [HttpPost]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Create([FromBody] RoleCreateDTO newRole)
        {
            RoleDetailDTO role = _roleService.AddRole(newRole);
            return Ok(new ApiResponse<RoleDetailDTO> { Result = role });
        }

        [HttpDelete("by-name/{name}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult DeleteByName(string name)
        {
            _roleService.DeleteRole(name);
            string Message = "Role deleted successfully.";
            return Ok(new ApiResponse<string> { Result = Message });
        }

        [HttpDelete("{id:int}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Delete(int id)
        {
            _roleService.DeleteRoleById(id);
            string Message = "Role deleted successfully.";
            return Ok(new ApiResponse<string> { Result = Message });
        }
    }
}
