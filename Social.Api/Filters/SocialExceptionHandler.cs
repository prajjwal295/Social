using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Social.Api.Contracts.Common.Response;

namespace Social.Api.Filters
{
    public class SocialExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var apiError = new ErrorResponse
            {
                StatusCode = 500,
                StatusPhase = "Internal Server Error",
                TimeStamp = DateTime.Now
            };

            apiError.Errors.Add(context.Exception.Message);
            context.Result = new JsonResult(apiError) { StatusCode = 500};
        }
    }
}
