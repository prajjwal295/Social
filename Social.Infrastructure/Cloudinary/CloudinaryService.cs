using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Social.Application.Options;

namespace Social.Infrastructure.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        private static readonly string[] AllowedTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        private const int MaxFileSize = 5 * 1024 * 1024;

        public CloudinaryService(
            IOptions<CloudinarySettings> options,
            ILogger<CloudinaryService> logger)
        {
            _logger = logger;

            var settings = options.Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            );

            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Invalid file.");
            }

            if (file.Length > MaxFileSize)
            {
                throw new Exception("File size exceeds 5MB.");
            }

            if (!AllowedTypes.Contains(file.ContentType))
            {
                throw new Exception("Invalid image type.");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Invalid file extension.");
            }

            var fileName = $"{Guid.NewGuid()}{extension}";

            try
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),

                    Folder = "users/profile-pictures",

                    UseFilename = false,
                    UniqueFilename = true,
                    Overwrite = false,

                    Transformation = new Transformation()
                        .Width(400)
                        .Height(400)
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto")
                        .FetchFormat("auto")
                };

                var uploadResult =
                    await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    _logger.LogError(
                        "Cloudinary upload failed: {Message}",
                        uploadResult.Error.Message);

                    throw new Exception(uploadResult.Error.Message);
                }

                return new ImageUploadResult
                {
                    Url = uploadResult.SecureUrl,
                    PublicId = uploadResult.PublicId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while uploading image.");

                throw;
            }
        }

        public async Task DeleteImageAsync(
            string publicId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(publicId))
            {
                return;
            }

            try
            {
                var deleteParams = new DeletionParams(publicId);

                var result =
                    await _cloudinary.DestroyAsync(deleteParams);

                if (result.Result != "ok")
                {
                    _logger.LogWarning(
                        "Failed to delete image with PublicId: {PublicId}",
                        publicId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while deleting image.");
            }
        }
    }
}