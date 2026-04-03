using MediaCatalog.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MediaCatalog.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            try
            {
                throw context.Exception;
            }
            catch (ResourceNotFoundException e)
            {
                context.Result = new JsonResult(new { message = e.Message, StatusCode = 404 }) { StatusCode = 404 };
            }
            catch (BadRequestException e)
            {
                context.Result = new JsonResult(new { message = e.Message, StatusCode = 400 }) { StatusCode = 400 };
            }
            catch (InvalidOperationException e)
            {
                context.Result = new JsonResult(new { message = e.Message, StatusCode = 409 }) { StatusCode = 409 };
            }
            catch (Exception e)
            {
                context.Result = new JsonResult(new { message = "An unexpected error occurred." })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
