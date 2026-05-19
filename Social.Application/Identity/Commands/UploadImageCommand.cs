using MediatR;
using Microsoft.AspNetCore.Http;
using Social.Application.DTO;
using Social.Application.Models;

namespace Social.Application.Identity.Commands
{
    public class UploadImageCommand :IRequest<OperationResult<UploadImageResponse>>
    {
        public IFormFile ImageFile { get; set; }
    }
}
