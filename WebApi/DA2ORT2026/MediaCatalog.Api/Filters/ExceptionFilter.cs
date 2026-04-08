using MediaCatalog.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MediaCatalog.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            if (exception is MediaCatalogException ex)
            {
                context.Result = new JsonResult(new
                {
                    message = ex.Message
                })
                {
                    StatusCode = ex.StatusCode
                };

                context.ExceptionHandled = true;
                return;
            }
            else
            {
                context.Result = new JsonResult(new
                {
                    message = "Internal Server Error"
                })
                {
                    StatusCode = 500
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
