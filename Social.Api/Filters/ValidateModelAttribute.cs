using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Social.Api.Contracts.Common.Response;

namespace Social.Api.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        //  the OnResultExcuting method basically is implemented when the result is been written
        public override void OnResultExecuting(ResultExecutingContext context) {

            if (!context.ModelState.IsValid)
            {
                {
                    var apiError = new ErrorResponse
                    {
                        StatusCode = 400,
                        StatusPhase = "Bad Request",
                        TimeStamp = DateTime.Now,
                    };

                    var errors = context.ModelState.AsEnumerable();

                    foreach (var error in errors)
                    {
                        foreach (var inner in error.Value.Errors)
                        {
                            apiError.Errors.Add(inner.ErrorMessage);
                        }
                    }

                    context.Result = new BadRequestObjectResult(apiError);
                }
            }
        }
    }
}
