using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Social.Api.Contracts.Common.Response;

namespace Social.Api.Filters
{
    public class ValidateGuidAttribute : ActionFilterAttribute
    {
        private readonly string _key;

        public ValidateGuidAttribute(string key)
        {
            _key = key;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue(_key, out var value))
            {
                return;
            }


            if (Guid.TryParse(value?.ToString(), out var guid))
            {
                return;
            }

            var apiError = new ErrorResponse
            {
                StatusCode = 400,
                StatusPhase = "Bad Request",
                TimeStamp = DateTime.Now
            };

            apiError.Errors.Add($"The Identifier for {_key} is not a correct Guid Format");
            context.Result = new ObjectResult(apiError);
        }
    }
}
