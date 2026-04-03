using MediaCatalog.Api.Filters;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
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
        public IActionResult GetAll()
        {
            List<RoleDetailDTO> roles = _roleService.GetRoles();
            return Ok(roles);
        }

        [HttpPost]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Create([FromBody] RoleCreateDTO newRole)
        {
            RoleDetailDTO createdRole = _roleService.AddRole(newRole);
            return Ok(createdRole);
        }

        [HttpDelete("{roleName}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Delete(string roleName)
        {
            _roleService.DeleteRole(roleName);
            return Ok(new { Message = "Role deleted successfully." });
        }
    }
}
