using Azure;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Common.Response;
using Social.Application.Enums;
using Social.Application.Models;

namespace Social.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult HandleErrorResponse(List<Error>errors)  {
            var apiError = new ErrorResponse();

            if (errors.Any(e => e.Code == ErrorCode.NotFound))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);
                apiError.StatusCode = 404;
                apiError.StatusPhase = "Not Found";
                apiError.TimeStamp = DateTime.Now;
                apiError.Errors.Add(error.Message);
                return NotFound(apiError);
            }

                apiError.StatusCode = 500;
                apiError.StatusPhase = "Internal Server Error";
                apiError.TimeStamp = DateTime.Now; 
                foreach (var e in errors)
                {
                    apiError.Errors.Add(e.Message);
                }
                return StatusCode(500,apiError);
        }
    }
}
