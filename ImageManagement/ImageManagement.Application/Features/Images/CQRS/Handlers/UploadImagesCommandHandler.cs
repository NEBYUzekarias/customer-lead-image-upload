using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Application.Features.Images.CQRS.Commands;
using ImageManagement.Application.Responses;
using ImageManagement.Application.Exceptions;
using ImageManagement.Domain;
using MediatR;
using System.Text.RegularExpressions;

namespace ImageManagement.Application.Features.Images.CQRS.Handlers
{
    public class UploadImagesCommandHandler : IRequestHandler<UploadImagesCommand, BaseCommandResponse>
    {
        private const int MaxImagesPerEntity = 10;
        private const int MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB
        private readonly IUnitOfWork _unitOfWork;

        public UploadImagesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(UploadImagesCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Validate that either CustomerId or LeadId is provided, but not both
                if ((request.Request.CustomerId == null && request.Request.LeadId == null) ||
                    (request.Request.CustomerId != null && request.Request.LeadId != null))
                {
                    response.Success = false;
                    response.Message = "Provide either CustomerId or LeadId, but not both.";
                    return response;
                }

                // Validate images are provided
                if (request.Request.Images == null || !request.Request.Images.Any())
                {
                    response.Success = false;
                    response.Message = "At least one image is required.";
                    return response;
                }

                // Validate entity exists and can accept more images
                int currentImageCount = 0;
                if (request.Request.CustomerId.HasValue)
                {
                    var customerExists = await _unitOfWork.CustomerRepository.CustomerExistsAsync(request.Request.CustomerId.Value);
                    if (!customerExists)
                    {
                        response.Success = false;
                        response.Message = $"Customer with ID {request.Request.CustomerId} not found.";
                        return response;
                    }
                    currentImageCount = await _unitOfWork.ProfileImageRepository.CountByCustomerAsync(request.Request.CustomerId.Value);
                }
                else if (request.Request.LeadId.HasValue)
                {
                    var leadExists = await _unitOfWork.LeadRepository.LeadExistsAsync(request.Request.LeadId.Value);
                    if (!leadExists)
                    {
                        response.Success = false;
                        response.Message = $"Lead with ID {request.Request.LeadId} not found.";
                        return response;
                    }
                    currentImageCount = await _unitOfWork.ProfileImageRepository.CountByLeadAsync(request.Request.LeadId.Value);
                }

                // Check if we can add more images
                var availableSlots = MaxImagesPerEntity - currentImageCount;
                if (availableSlots <= 0)
                {
                    response.Success = false;
                    response.Message = $"Maximum of {MaxImagesPerEntity} images allowed. Currently have {currentImageCount} images.";
                    return response;
                }

                // Validate and process each image
                var imagesToAdd = new List<ProfileImage>();
                var processedCount = 0;
                bool hasMainImage = false;

                foreach (var imageDto in request.Request.Images.Take(availableSlots))
                {
                    // Validate Base64 format
                    if (string.IsNullOrWhiteSpace(imageDto.Base64Data))
                    {
                        response.Errors.Add($"Image {processedCount + 1}: Base64 data is required.");
                        continue;
                    }

                    // Extract content type and data from Base64 string if it includes data URL prefix
                    string base64Data = imageDto.Base64Data;
                    string? contentType = imageDto.ContentType;

                    if (base64Data.StartsWith("data:"))
                    {
                        var match = Regex.Match(base64Data, @"^data:([^;]+);base64,(.+)$");
                        if (match.Success)
                        {
                            contentType = match.Groups[1].Value;
                            base64Data = match.Groups[2].Value;
                        }
                    }

                    // Validate Base64 format
                    try
                    {
                        var imageBytes = Convert.FromBase64String(base64Data);
                        
                        // Validate image size
                        if (imageBytes.Length > MaxImageSizeBytes)
                        {
                            response.Errors.Add($"Image {processedCount + 1}: File size exceeds maximum allowed size of {MaxImageSizeBytes / (1024 * 1024)}MB.");
                            continue;
                        }

                        // Create ProfileImage entity
                        var profileImage = new ProfileImage
                        {
                            Base64Data = base64Data,
                            ContentType = contentType,
                            FileName = imageDto.FileName,
                            Description = imageDto.Description,
                            FileSize = imageBytes.Length,
                            IsMainImage = imageDto.IsMainImage && !hasMainImage, // Only first main image is set
                            CustomerId = request.Request.CustomerId,
                            LeadId = request.Request.LeadId
                        };

                        if (profileImage.IsMainImage)
                        {
                            hasMainImage = true;
                        }

                        imagesToAdd.Add(profileImage);
                        processedCount++;
                    }
                    catch (FormatException)
                    {
                        response.Errors.Add($"Image {processedCount + 1}: Invalid Base64 format.");
                        continue;
                    }
                }

                if (!imagesToAdd.Any())
                {
                    response.Success = false;
                    response.Message = "No valid images to upload.";
                    return response;
                }

                // Add images to repository
                foreach (var image in imagesToAdd)
                {
                    await _unitOfWork.ProfileImageRepository.Add(image);
                }

                await _unitOfWork.Save();

                response.Success = true;
                response.Message = $"Successfully uploaded {imagesToAdd.Count} image(s).";
                response.Id = imagesToAdd.Count; // Return count of uploaded images

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while uploading images.";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
    }
}
