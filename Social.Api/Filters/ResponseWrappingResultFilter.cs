using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Social.Api.Filters
{
    public class ResponseWrappingResultFilter : ResultFilterAttribute
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult
                && objectResult.StatusCode is null or (>= 200 and < 300))
            {
                objectResult.Value = new
                {
                    success = true,
                    data = objectResult.Value,
                    timestamp = DateTime.UtcNow
                };
            }
        }

    }
}
