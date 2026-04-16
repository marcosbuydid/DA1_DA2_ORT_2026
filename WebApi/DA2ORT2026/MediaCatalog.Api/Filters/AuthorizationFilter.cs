using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MediaCatalog.Api.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private List<string> _roles;

        public AuthorizationFilter(string role = "")
        {
            _roles = role.Split(",").ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ISessionService sessionService = GetSessionService(context);

            string? token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            SessionDTO? currentSession = sessionService.ValidateSession(token);

            if (currentSession == null)
            {
                context.Result = new JsonResult(new { Token = "Invalid Token" })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                string? loggedUserRoleName = currentSession.LoggedUserRoleName;

                if (String.IsNullOrEmpty(loggedUserRoleName) || !_roles.Contains(loggedUserRoleName))
                {
                    context.Result = new JsonResult(new { Status = "Unauthorized to perform this action" })
                    { StatusCode = StatusCodes.Status403Forbidden };
                }
            }
        }

        protected ISessionService GetSessionService(AuthorizationFilterContext context)
        {
            var sessionHandlerType = typeof(ISessionService);
            var sessionHandlerLogicObject = context.HttpContext.RequestServices.GetService(sessionHandlerType);
            var sessionHandler = sessionHandlerLogicObject as ISessionService;

            return sessionHandler;
        }
    }
}
