using MediatR;
using Social.Application.DTO;
using Social.Application.Identity.Commands;
using Social.Application.Models;
using Social.Infrastructure.Cloudinary;

namespace Social.Application.Identity.Handlers
{
    internal class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, OperationResult<UploadImageResponse>>
    {
        private readonly ICloudinaryService _cloudinaryService;

        public UploadImageCommandHandler(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<OperationResult<UploadImageResponse>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UploadImageResponse>();
            try
            {
                UploadImageResponse uploadImageResponse = new UploadImageResponse();
                var uploadResult = await _cloudinaryService
                   .UploadImageAsync(
                       request.ImageFile,
                       cancellationToken);

                uploadImageResponse.Url = uploadResult.Url.ToString();
                uploadImageResponse.PublicId = uploadResult.PublicId;

                result.Payload = uploadImageResponse;

            }
            catch (Exception ex)
            { 
                result.IsError = true;
                var error = new Error
                {
                    Code = Enums.ErrorCode.UnknownError,
                    Message = ex.Message
                };
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
