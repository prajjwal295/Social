using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Social.Infrastructure.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default);

        Task DeleteImageAsync(string publicId, CancellationToken cancellationToken = default);
    }
}
