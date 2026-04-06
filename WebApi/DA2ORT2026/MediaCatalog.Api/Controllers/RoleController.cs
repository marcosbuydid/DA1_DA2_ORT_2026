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
        [AuthorizationFilter("Administrator,User")]
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

        [HttpDelete("by-name/{name}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult DeleteByName(string name)
        {
            _roleService.DeleteRole(name);
            return Ok(new { Message = "Role deleted successfully." });
        }

        [HttpDelete("{id:int}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Delete(int id)
        {
            _roleService.DeleteRoleById(id);
            return Ok(new { Message = "Role deleted successfully." });
        }
    }
}
